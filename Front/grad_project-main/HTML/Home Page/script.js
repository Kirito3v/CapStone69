angular.module('cyberStore', []).controller('ProductController', function($scope) {
    $scope.products = [
        { name: 'iPhone 14 Pro Max', price: 900, image: 'iphone.jpg' },
        { name: 'Blackmagic Camera', price: 2535, image: 'camera.jpg' },
        { name: 'Apple Watch Series 8', price: 399, image: 'watch.jpg' }
    ];
    
    $scope.categories = ['Phones', 'Smart Watches', 'Cameras', 'Headphones', 'Computers', 'Gaming'];
});

function searchFunction(event) {
    event.preventDefault();
    let query = document.getElementById('searchInput').value;
    alert('Searching for: ' + query);
}


document.addEventListener("DOMContentLoaded", function () {
    let observer = new IntersectionObserver(entries => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                document.querySelectorAll(".category-box").forEach((box, index) => {
                    setTimeout(() => {
                        box.classList.add("animate");
                    }, index * 200);
                });
            }
        });
    }, { threshold: 0.3 });
    observer.observe(document.querySelector("#categorySection"));
});




function showSection(sectionId, element) {
    document.querySelectorAll('.products-section').forEach(section => {
        section.classList.remove('active');
    });
    document.getElementById(sectionId).classList.add('active');
    
    document.querySelectorAll('.nav-link').forEach(link => {
        link.classList.remove('active');
    });
    element.classList.add('active');
}