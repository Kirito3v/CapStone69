function changeImage(element) {
    document.getElementById('main-image').src = element.src;
    document.querySelectorAll('.thumbnail-img').forEach(img => img.classList.remove('active'));
    element.classList.add('active');
}
function selectColor(element, color) {
    document.querySelectorAll('.color-option').forEach(opt => opt.classList.remove('selected'));
    element.classList.add('selected');
    console.log("Selected Color:", color);
}
function selectStorage(element, size) {
    document.querySelectorAll('.storage-option').forEach(btn => btn.classList.remove('selected'));
    element.classList.add('selected');
    console.log("Selected Storage:", size);
}