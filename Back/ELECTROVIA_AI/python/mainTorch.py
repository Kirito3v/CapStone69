import asyncio
import os
import numpy as np
import faiss
from typing import Dict
from PIL import Image
from tqdm import tqdm



os.environ['TF_ENABLE_ONEDNN_OPTS'] = '0'
os.environ["KMP_DUPLICATE_LIB_OK"] = "TRUE"

import torch
torch.backends.cudnn.benchmark = True

import torch.nn as nn
import torch.optim as optim
from torch.utils.data import DataLoader, Dataset
from torchvision import transforms, models  #type: ignore
from torchvision.datasets import ImageFolder  #type: ignore
from torch.cuda.amp import GradScaler, autocast



# Global variables
input_shape = (224, 224, 3)
batch_size = 64
epochs = 30
iterations = 1
learning_rate = 0.003
cancelToken = False

# Dictionaries
indexes: Dict[str, faiss.IndexFlatL2] = {}
generators: Dict[str, tuple] = {}
feature_extractors: Dict[str, nn.Module] = {}
features_dic: Dict[str, np.ndarray] = {}
labels_dic: Dict[str, np.ndarray] = {}

# Model definition
class CustomModel(nn.Module):
    def __init__(self, num_classes):
        super(CustomModel, self).__init__()
        

        self.base_model = models.mobilenet_v2(weights=models.MobileNet_V2_Weights.DEFAULT).features


        for param in self.base_model.parameters():
            param.requires_grad = False
            
        self.global_avg_pool = nn.AdaptiveAvgPool2d(1)
        self.flatten = nn.Flatten()
        self.dense = nn.Linear(1280, 128)
        self.relu = nn.ReLU()
        self.classifier = nn.Linear(128, num_classes)

    def forward(self, x):
        x = self.base_model(x)
        x = self.global_avg_pool(x)
        x = self.flatten(x)
        x = self.dense(x)
        x = self.relu(x)
        return self.classifier(x)

# Feature extractor definition
class FeatureExtractor(nn.Module):
    def __init__(self, original_model):
        super(FeatureExtractor, self).__init__()
        self.base_model = original_model.base_model
        self.global_avg_pool = original_model.global_avg_pool
        self.flatten = original_model.flatten
        self.dense = original_model.dense
        self.relu = original_model.relu

    def forward(self, x):
        x = self.base_model(x)
        x = self.global_avg_pool(x)
        x = self.flatten(x)
        x = self.dense(x)
        x = self.relu(x)
        return x

def init(directory, generator_name):

    print("building model")
    transform = transforms.Compose([
        transforms.Resize((224, 224)),
        transforms.ToTensor(),
    ])

    full_dataset = ImageFolder(root=directory, transform=transform)
    class_to_idx = full_dataset.class_to_idx
    classes = full_dataset.classes


    # Split dataset
    train_size = int(0.8 * len(full_dataset))
    val_size = len(full_dataset) - train_size
    train_dataset, _ = torch.utils.data.random_split(
        full_dataset, [train_size, val_size],
        generator=torch.Generator().manual_seed(42))
    
    train_loader = DataLoader(train_dataset, batch_size=batch_size, shuffle=True, num_workers=8,persistent_workers=True,pin_memory=True)
    
    generators[generator_name] = (train_loader, class_to_idx, classes)
    
    model_path = f"Torch_feature_extractor_{generator_name}.pt"

    if not os.path.exists(model_path):
        # Initialize and train model
        model = CustomModel(len(classes)).to(device)
        criterion = nn.CrossEntropyLoss()
        optimizer = optim.Adam(model.parameters(), lr=learning_rate)
        scheduler = optim.lr_scheduler.CosineAnnealingLR(optimizer, T_max=epochs)

        # Training loop
        scaler = GradScaler()
        model.train()
        for epoch in range(epochs):
            running_loss = 0.0
            correct = 0
            total = 0
            with tqdm(train_loader, desc=f"Epoch {epoch+1}/{epochs}") as pbar:
                for inputs, labels in pbar:
                    inputs = inputs.to(device, non_blocking=True)
                    labels = labels.to(device, non_blocking=True)
                    optimizer.zero_grad()
                    with autocast():
                        outputs = model(inputs)
                        loss = criterion(outputs, labels)
                    scaler.scale(loss).backward()
                    scaler.step(optimizer)
                    scaler.update()
            
                    # metrics
                    running_loss += loss.item()
                    _, preds = outputs.detach().max(1)
                    total += labels.size(0)
                    correct += (preds == labels).sum().item()
                    pbar.set_postfix({
                        "loss": f"{running_loss/total:.4f}",
                        "acc": f"{correct/total:.4f}"
                    })
            scheduler.step()
        # Save feature extractor
        feature_extractor = FeatureExtractor(model).eval()
        torch.save(feature_extractor.state_dict(), model_path)

        # Extract features
        features = []
        All_labels = []

        with torch.no_grad():
            for inputs, labels in train_loader:
                features.append(feature_extractor(inputs.to(device)).cpu())
                All_labels.extend(t.item() for t in labels)
        features = torch.cat(features).numpy()
        features_dic[generator_name] = features
        np.save(f"Torch_features_{generator_name}.npy", features)

        labels_dic[generator_name] = All_labels
        np.save(f"Torch_{generator_name}_labels.npy", All_labels)
        feature_extractors[generator_name] = feature_extractor
        
    else:
        # Load existing model
        model = CustomModel(len(classes)).to(device)
        feature_extractor = FeatureExtractor(model).eval()
        feature_extractor.load_state_dict(torch.load(model_path))
        feature_extractors[generator_name] = feature_extractor
        
        features_dic[generator_name] = np.load(f"Torch_features_{generator_name}.npy")
        labels_dic[generator_name] = np.load(f"Torch_{generator_name}_labels.npy")



    # Create FAISS index
    index = faiss.IndexFlatL2(features_dic[generator_name].shape[1])
    index.add(features_dic[generator_name])
    indexes[generator_name] = index
    print("loaded")

def ImageSearch(generator_name):
    if generator_name not in generators:
        print("Generator not found")
        print(f"avilable generators({generators.keys()})")
        return

    train_loader, class_to_idx, classes = generators[generator_name]
    idx_to_class = {v: k for k, v in class_to_idx.items()}
    feature_extractor = feature_extractors[generator_name]
    index = indexes[generator_name]
    features = features_dic[generator_name]
    labels = labels_dic[generator_name]
    k = len(classes)

    image_path = input()

    img_tensor = load_and_preprocess_image(image_path)
    
    with torch.no_grad():
        query_feature = feature_extractor(img_tensor.to(device)).cpu().numpy().flatten()

    distances, indices = index.search(np.array([query_feature]), k=iterations * len(features))
    added_names = []
    
    for idx in indices[0]:
        predicted_idx = labels[idx]
        class_name = idx_to_class[predicted_idx]

        if class_name not in added_names:
            added_names.append(class_name)
            if len(added_names) >= k:
                break
    

    print("ready")
    print(','.join(added_names))

def load_and_preprocess_image(image_path):
    transform = transforms.Compose([
        transforms.Resize((224, 224)),
        transforms.ToTensor(),
    ])
    img = Image.open(image_path).convert('RGB')
    return transform(img).unsqueeze(0)

def ImageSearchFromGenerator():

    generator_name = input()

    ImageSearch(generator_name)

def init_Model():
    directory = input()
    generator_name = input()

    init(directory,generator_name)

async def main():
    global cancelToken

    while not cancelToken:

        methodName = input()

        if methodName in globals():
            globals()[methodName]() # run method
            methodName =None
        else:
            print("not found")
            
if __name__ == "__main__":    

    device = torch.device("cuda" if torch.cuda.is_available() else "cpu")

    os.chdir("bin/Debug/net8.0")

    asyncio.run(main())

print("exited!")