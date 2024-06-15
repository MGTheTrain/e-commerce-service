import requests
import uuid
import pytest
import os

API_BASE_URL = os.getenv('API_BASE_URL', 'http://localhost/api/v1')
TEST_CATEGORY_NAME = "Test Category"
TEST_CATEGORY_DESCRIPTION = "This is a test category."
TEST_PRODUCT_NAME = "Test Product"
TEST_PRODUCT_DESCRIPTION = "This is a test product."
TEST_PRODUCT_PRICE = 99.99
TEST_PRODUCT_STOCK = 10
TEST_PRODUCT_IMAGE_URL = "http://example.com/test-product.jpg"

@pytest.fixture(scope="module")
def category_id():
    url = f"{API_BASE_URL}/categories"
    body = {'name': "New Test Category", 'description': "This is another test category."}
    response = requests.post(url, json=body)
    print('Create Category Response:', response.status_code)
    assert response.status_code == 201
    return response.json().get('categoryID')

@pytest.fixture(scope="module")
def product_id(category_id):
    url = f"{API_BASE_URL}/products"
    body = {
        'categoryID': category_id,
        'name': "New Test Product",
        'description': "This is another test product.",
        'price': 49.99,
        'stock': 20,
        'imageUrl': "http://example.com/new-test-product.jpg"
    }
    response = requests.post(url, json=body)
    print('Create Product Response:', response.status_code)
    assert response.status_code == 201
    return response.json().get('productID')

def test_create_category():
    url = f"{API_BASE_URL}/categories"
    body = {'name': "New Test Category", 'description': "This is another test category."}
    response = requests.post(url, json=body)
    print('Create Category Response:', response.status_code)
    assert response.status_code == 201
    assert 'categoryID' in response.json()

def test_get_all_categories():
    url = f"{API_BASE_URL}/categories"
    response = requests.get(url)
    print('Get All Categories Response:', response.status_code)
    assert response.status_code == 200
    assert isinstance(response.json(), list)

def test_get_category_by_id(category_id):
    url = f"{API_BASE_URL}/categories/{category_id}"
    response = requests.get(url)
    print('Get Category By ID Response:', response.status_code)
    assert response.status_code == 200
    assert response.json().get('categoryID') == category_id

def test_update_category(category_id):
    url = f"{API_BASE_URL}/categories/{category_id}"
    body = {'name': "Updated Test Category", 'description': "This is an updated test category."}
    response = requests.put(url, json=body)
    print('Update Category Response:', response.status_code)
    assert response.status_code == 200
    updated_category = response.json()
    assert updated_category.get('name') == "Updated Test Category"
    assert updated_category.get('description') == "This is an updated test category."

def test_delete_category(category_id):
    url = f"{API_BASE_URL}/categories/{category_id}"
    response = requests.delete(url)
    print('Delete Category Response:', response.status_code)
    assert response.status_code == 204

    # Verify the category was deleted
    response = requests.get(url)
    assert response.status_code == 404

def test_create_product(category_id):
    url = f"{API_BASE_URL}/products"
    body = {
        'categoryID': category_id,
        'name': "New Test Product",
        'description': "This is another test product.",
        'price': 49.99,
        'stock': 20,
        'imageUrl': "http://example.com/new-test-product.jpg"
    }
    response = requests.post(url, json=body)
    print('Create Product Response:', response.status_code)
    assert response.status_code == 201
    assert 'productID' in response.json()

def test_get_all_products():
    url = f"{API_BASE_URL}/products"
    response = requests.get(url)
    print('Get All Products Response:', response.status_code)
    assert response.status_code == 200
    assert isinstance(response.json(), list)

def test_get_product_by_id(product_id):
    url = f"{API_BASE_URL}/products/{product_id}"
    response = requests.get(url)
    print('Get Product By ID Response:', response.status_code)
    assert response.status_code == 200
    assert response.json().get('productID') == product_id

def test_update_product(product_id):
    url = f"{API_BASE_URL}/products/{product_id}"
    body = {
        'categoryID': str(uuid.uuid4()),  # Example: Updating to a new category ID
        'name': "Updated Test Product",
        'description': "This is an updated test product.",
        'price': 59.99,
        'stock': 15,
        'imageUrl': "http://example.com/updated-test-product.jpg"
    }
    response = requests.put(url, json=body)
    print('Update Product Response:', response.status_code)
    assert response.status_code == 200
    updated_product = response.json()
    assert updated_product.get('name') == "Updated Test Product"
    assert updated_product.get('description') == "This is an updated test product."
    assert updated_product.get('price') == 59.99
    assert updated_product.get('stock') == 15
    assert updated_product.get('imageUrl') == "http://example.com/updated-test-product.jpg"

def test_delete_product(product_id):
    url = f"{API_BASE_URL}/products/{product_id}"
    response = requests.delete(url)
    print('Delete Product Response:', response.status_code)
    assert response.status_code == 204

    # Verify the product was deleted
    response = requests.get(url)
    assert response.status_code == 404
