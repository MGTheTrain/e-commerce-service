import requests
import uuid
import pytest
import os

API_BASE_URL = os.getenv('API_BASE_URL', 'http://localhost/api/v1')
BEARER_TOKEN = os.getenv('BEARER_TOKEN', '')
TEST_CATEGORY_NAME = "Test Category"
TEST_CATEGORY_DESCRIPTION = "This is a test category."
TEST_PRODUCT_NAME = "Test Product"
TEST_PRODUCT_DESCRIPTION = "This is a test product."
TEST_PRODUCT_PRICE = 99.99
TEST_PRODUCT_STOCK = 10
TEST_PRODUCT_IMAGE_URL = "http://example.com/test-product.jpg"

@pytest.fixture(scope="module")
def product_id():
    url = f"{API_BASE_URL}/products"
    headers = {
        'Authorization': f'Bearer {BEARER_TOKEN}'
    }
    body = {
        'categories': [str(uuid.uuid4())],  # Example: List of category IDs
        'name': "New Test Product",
        'description': "This is another test product.",
        'price': 4999.99,
        'stock': 20,
        'imageUrl': "http://example.com/new-test-product.jpg"
    }
    response = requests.post(url, json=body, headers=headers)
    print('Create Product Response:', response.status_code)
    assert response.status_code == 201
    return response.json().get('productID')

def test_create_product():
    url = f"{API_BASE_URL}/products"
    headers = {
        'Authorization': f'Bearer {BEARER_TOKEN}'
    }
    body = {
        'categories': [str(uuid.uuid4())],  # Example: List of category IDs
        'name': "New Test Product",
        'description': "This is another test product.",
        'price': 4999.99,
        'stock': 20,
        'imageUrl': "http://example.com/new-test-product.jpg"
    }
    response = requests.post(url, json=body, headers=headers)
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
    headers = {
        'Authorization': f'Bearer {BEARER_TOKEN}'
    }
    body = {
        'categories': [str(uuid.uuid4())],  # Example: List of new category IDs
        'name': "Updated Test Product",
        'description': "This is an updated test product.",
        'price': 59.99,
        'stock': 15,
        'imageUrl': "http://example.com/updated-test-product.jpg"
    }
    response = requests.put(url, json=body, headers=headers)
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
    headers = {
        'Authorization': f'Bearer {BEARER_TOKEN}'
    }
    response = requests.delete(url, headers=headers)
    print('Delete Product Response:', response.status_code)
    assert response.status_code == 204

    # Verify the product was deleted
    response = requests.get(url)
    assert response.status_code == 404
