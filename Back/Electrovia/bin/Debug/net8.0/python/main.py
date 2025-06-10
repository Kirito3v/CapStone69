import asyncio

import os
from tabnanny import verbose
import warnings

#os.chdir("bin/Debug/net8.0")
os.environ['TF_ENABLE_ONEDNN_OPTS'] = '0'
os.environ['TF_CPP_MIN_LOG_LEVEL'] = '2'

from tensorflow.keras.models import load_model,Model #type: ignore
from tensorflow.keras.preprocessing.image import ImageDataGenerator,load_img,img_to_array,DirectoryIterator #type: ignore
import faiss 
import sys
import io

sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')

import numpy as np

import tensorflow as tf

warnings.filterwarnings("ignore", category=DeprecationWarning)
tf.get_logger().setLevel('ERROR')
    
from tensorflow.keras.applications import MobileNetV2 #type: ignore
from tensorflow.keras.layers import Dropout, Flatten, Dense,GlobalAveragePooling2D  #type: ignore
from tensorflow.keras.optimizers import Adam  #type: ignore
    

from typing import Dict

# process terminator

cancelToken = False

# variables

input_shape = (224, 224, 3)
 
batch_size = 64
epochs = 30
iter = 1
learning_rate = 0.003


#data generator



IDG = ImageDataGenerator(
    rescale=1.0/255.0,
    validation_split=0.2
)




#dictionaries

indexes: Dict[str,faiss.IndexFlatL2] = \
{
    
}

generators: Dict[str, DirectoryIterator] = \
{
    
}


feature_extractors: Dict[str,Model] = \
{
    
}


features_dic: Dict[str,faiss.IndexFlatL2]=\
{
    
}

labels_dic: Dict[str,list] = \
{
    
} 

#model and generator initializer


def init(directory,generator_name):
    

    generator = IDG.flow_from_directory(
        directory=directory,
        target_size=(input_shape[0],input_shape[1]),
        batch_size= batch_size,
        class_mode= "categorical",
        subset="training",
        shuffle=True,
        seed=42,
        color_mode='rgb' 

    )
    
    generators[generator_name] = generator
    
    if not os.path.exists(f"feature_extractor_{generator_name}.keras"):


        #creating base model

        k = len(generator.class_indices)
        base_model  = MobileNetV2(weights='imagenet', include_top=False, input_shape=input_shape)

        base_model.trainable = False


        x = base_model.output 
        x = GlobalAveragePooling2D()(x) 
        x = Dense(128, activation='relu')(x)  
        # x = Dropout(0.5)(x)
        output = Dense(k, activation='softmax')(x)
    
        model = Model(inputs=base_model.input, outputs=output)

        model.compile(loss='categorical_crossentropy', optimizer=Adam(learning_rate=learning_rate), metrics=['accuracy'])

        #generator.repeat()  
        model.fit(generator, epochs=epochs,verbose=1)
        
        
        test_loss, test_accuracy = model.evaluate(generator)

        print("Loss:", test_loss)
        print("Accuracy:", test_accuracy)
    

    
        features_extractor = Model(inputs=model.input, outputs=model.layers[-2].output)
        features = features_extractor.predict(generator)
        
        features_dic[generator_name] = features
        
        features_extractor.save(f"feature_extractor_{generator_name}.keras")
        
        np.save(f"features_{generator_name}.npy",features)
        
        feature_extractors[generator_name] = features_extractor
        
        classifier_input = model.layers[-2].output  # Use the output of the second to last layer
        classifier_output = model.layers[-1].output

        label_getter = Model(inputs=classifier_input, outputs=classifier_output)
        labels = label_getter.predict(features)
        
        np.save(f"{generator_name}_labels.npy",labels)
        
        labels_dic[generator_name] = labels
        
    else:
        feature_extractors[generator_name] = load_model(f"feature_extractor_{generator_name}.keras")
        features_dic[generator_name] = np.load(f"features_{generator_name}.npy")
        labels_dic[generator_name] = np.load(f"{generator_name}_labels.npy")
        
    
    features = features_dic[generator_name]
    index = faiss.IndexFlatL2(features_dic[generator_name].shape[1])
    index.add(features)
    indexes[generator_name] = index
    print("loaded")
        
        
def ImageSearchFromGenerator():
    generator_name = input()
    ImageSearch(generator_name)

def ImageSearch(generator_name):

    generator = generators[generator_name]
    
    features_extractor = feature_extractors[generator_name]
    
    features = features_dic[generator_name]
    index = indexes[generator_name]
    labels = labels_dic[generator_name]
    num_samples = generator.samples * iter

    image_path = input()

    img_array =load_and_preprocess_image(image_path)
    
    Img_features = features_extractor.predict(img_array).flatten()

  

    d, I = index.search(np.array([Img_features]), k=num_samples)

    print(d[0])
    k = len(generator.class_indices)

    class_indices = generator.class_indices  
    classes = {v: k for k, v in class_indices.items()}  
    i =0

    ######
  

    added_names = []
    for idx in I[0]:

        
        predicted_label = np.argmax(labels[idx])

        prediction_name = classes.get(predicted_label, "Un known")
        
        if(not added_names.__contains__(prediction_name)):
            added_names.append(prediction_name)
            i+=1
        if(i>k-1):
            break
    print("ready")

    print(','.join(added_names))

def load_and_preprocess_image(image_path):
    img = load_img(image_path,target_size = input_shape)
    img_array = img_to_array(img) / 255.0  
    img_array = np.expand_dims(img_array, axis=0)  
    return img_array

def init_Model():
    directory = input()
    generator_name = input()

    init(directory,generator_name)
async def main():
    os.chdir("bin/Debug/net8.0")
    global cancelToken
    while not cancelToken:
        methodName = input()
        if methodName in globals():
            globals()[methodName]() # run method
        else:
            print("not found")

asyncio.run(main())