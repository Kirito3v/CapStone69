var conBE = "https://localhost:7228/api/products";

async function fetchData() {
    try {
        const response = await fetch(conBE);
        const products = await response.json();

        const productsDiv = document.getElementById('products');
        productsDiv.innerHTML = products.map(product => `
            <div class="product">
                <h3>${product.name}</h3>
                <p>Price: $${product.price}</p>
            </div>
        `).join('');
    } catch (error) {
        console.error('Error fetching data:', error);
        alert('Failed to load products!');
    }
}

// بصوا هوا فيه طريقتين هنا الطريقة اللي معمولة بالجافا سكريبت
// و الطريقة اللي معمولة بالانجولار

// Call this when page loads
document.addEventListener('DOMContentLoaded', async function () {
    const products = await fetchData();
    renderProducts(products);
});

// دي الطريقة اللي معمولة بالجافا سكريبت
function renderProducts(products) {
    const container = document.getElementById('product-tabs');

    // هنا ظبطوها بقى
    // Example rendering - adjust to match your HTML structure
    products.forEach(product => {
        const productCard = document.createElement('div');
        productCard.className = 'product-card';
        productCard.innerHTML = `
            <img src="${product.image}" alt="${product.name}">
            <h6>${product.name}</h6>
            <div class="price">$${product.price}</div>
            <a href="../product details/${product.id}" class="btn-buy">Buy Now</a>
        `;
        container.appendChild(productCard);
    });
}


// و دي الطريقة اللي معمولة بالانجولار
// angular.module('ELECTROVIA', [])
//     .service('ApiService', function ($http) {
//         this.getProducts = function () {
//             return $http.get(conBE);
//         };
//         this.addToCart = function (productId) {
//             return $http.post(conBE, { productId });
//         };
//     })
//     .controller('MainController', function ($scope, ApiService) {
//         // Fetch products on page load
//         ApiService.getProducts().then(response => {
//             $scope.products = response.data;
//         });
//     }).controller('MainController', function ($scope, $http) {
//         $scope.fetchData = async function () {
//             try {
//                 const response = await $http.get(conBE);
//                 $scope.products = response.data;
//                 $scope.$apply(); // Trigger digest cycle
//             } catch (error) {
//                 console.error('Error fetching data:', error);
//             }
//         };

//         // Call on controller initialization
//         $scope.fetchData();
//     });







document.addEventListener('DOMContentLoaded', function () {
    // Mobile menu toggle
    const toggle = document.getElementById('menu-toggle');
    const navLinks = document.getElementById('nav-links');
    toggle.addEventListener('click', () => navLinks.classList.toggle('active'));

    // Cart count update
    updateCartCount();
});

// لو عاوزين تستخدموا الحته براحتكم
function updateCartCount() {
    const count = localStorage.getItem('cartCount') || 0;
    document.querySelectorAll('.cart-count').forEach(el => el.textContent = count);
}

function addToCart(productId) {
    const cart = JSON.parse(localStorage.getItem('cart')) || [];
    cart.push(productId);
    localStorage.setItem('cart', JSON.stringify(cart));
    updateCartCount();
}

function showSection(sectionId, clickedTab) {
    document.querySelectorAll('.nav-link').forEach(tab => tab.classList.remove('active'));
    document.querySelectorAll('.product-section').forEach(section => section.classList.remove('active'));

    clickedTab.classList.add('active');
    document.getElementById(sectionId).classList.add('active');
}






// document.getElementById('productForm').onsubmit = async (e) => {
//     e.preventDefault();

//     const product = {
//         name: document.getElementById('name').value,
//         price: document.getElementById('price').value
//     };

//     try {
//         const response = await fetch('https://localhost:7228/api/products', {
//             method: 'POST',
//             headers: { 'Content-Type': 'application/json' },
//             body: JSON.stringify(product)
//         });
//         alert('Product added!');
//     } catch (error) {
//         console.error('Error:', error);
//     }
// };