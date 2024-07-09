import requests
import uuid
import pytest
import os

API_BASE_URL = os.getenv('API_BASE_URL', 'http://localhost:7260/api/v1')
BEARER_TOKEN = os.getenv('BEARER_TOKEN', '')
TEST_USER_ID = str(uuid.uuid4())  # Sample UUID for test

@pytest.fixture(scope="module")
def cart_data():
    # Setup: Create a cart and return its data (user ID and cart ID)
    url = f"{API_BASE_URL}/carts"
    headers = {
        'Authorization': f'Bearer {BEARER_TOKEN}'
    }
    body = {'userID': TEST_USER_ID, 'totalAmount': 100.0}
    response = requests.post(url, json=body, headers=headers) 
    assert response.status_code == 201
    cart_id = response.json().get('cartID')
    return {'user_id': TEST_USER_ID, 'cart_id': cart_id}

@pytest.fixture(scope="module")
def cart_item_id(cart_data):
    # Setup: Create a cart item and return its ID
    url = f"{API_BASE_URL}/carts/{cart_data['cart_id']}/items"
    headers = {
        'Authorization': f'Bearer {BEARER_TOKEN}'
    }
    body = {
        'productID': str(uuid.uuid4()),  # Sample product ID
        'quantity': 1,
        'price': 50.0
    }
    response = requests.post(url, json=body, headers=headers)
    assert response.status_code == 201
    return response.json().get('cartItemID')

def test_create_cart():
    url = f"{API_BASE_URL}/carts"
    headers = {
        'Authorization': f'Bearer {BEARER_TOKEN}'
    }
    body = {'userID': TEST_USER_ID, 'totalAmount': 100.0}
    response = requests.post(url, json=body, headers=headers)
    assert response.status_code == 201
    assert 'cartID' in response.json()

def test_get_cart(cart_data):
    url = f"{API_BASE_URL}/carts/{cart_data['cart_id']}"
    headers = {
        'Authorization': f'Bearer {BEARER_TOKEN}'
    }
    response = requests.get(url, headers=headers)
    print('Get Cart Response:', response.status_code)
    assert response.status_code == 200

def test_update_cart(cart_data):
    url = f"{API_BASE_URL}/carts/{cart_data['cart_id']}"
    headers = {
        'Authorization': f'Bearer {BEARER_TOKEN}'
    }
    body = {'totalAmount': 150.0}
    response = requests.put(url, json=body, headers=headers)
    print('Update Cart Response:', response.status_code)
    assert response.status_code == 200

def test_add_item_to_cart(cart_data):
    url = f"{API_BASE_URL}/carts/{cart_data['cart_id']}/items"
    headers = {
        'Authorization': f'Bearer {BEARER_TOKEN}'
    }
    body = {
        'productID': str(uuid.uuid4()),  # Sample product ID
        'quantity': 1,
        'price': 50.0
    }
    response = requests.post(url, json=body, headers=headers)
    print('Add Item to Cart Response:', response.status_code)
    assert response.status_code == 201

def test_get_cart_items(cart_data):
    url = f"{API_BASE_URL}/carts/{cart_data['cart_id']}/items"
    headers = {
        'Authorization': f'Bearer {BEARER_TOKEN}'
    }
    response = requests.get(url, headers=headers)
    print('Get Cart Items Response:', response.status_code)
    assert response.status_code == 200

def test_get_cart_item_by_id(cart_data, cart_item_id):
    url = f"{API_BASE_URL}/carts/{cart_data['cart_id']}/items/{cart_item_id}"
    headers = {
        'Authorization': f'Bearer {BEARER_TOKEN}'
    }
    response = requests.get(url, headers=headers)
    print('Get Cart Item by ID Response:', response.status_code)
    assert response.status_code == 200

def test_update_cart_item(cart_data, cart_item_id):
    url = f"{API_BASE_URL}/carts/{cart_data['cart_id']}/items/{cart_item_id}"
    headers = {
        'Authorization': f'Bearer {BEARER_TOKEN}'
    }
    body = {
        'productID': str(uuid.uuid4()),  # Sample product ID
        'quantity': 2,
        'price': 100.0
    }
    response = requests.put(url, json=body, headers=headers)
    print('Update Cart Item Response:', response.status_code)
    assert response.status_code == 200

def test_delete_cart_item(cart_data, cart_item_id):
    url = f"{API_BASE_URL}/carts/{cart_data['cart_id']}/items/{cart_item_id}"
    headers = {
        'Authorization': f'Bearer {BEARER_TOKEN}'
    }
    response = requests.delete(url, headers=headers)
    print('Delete Cart Item Response:', response.status_code)
    assert response.status_code == 204

    # Verify the cart item was deleted
    response = requests.get(url, headers=headers)
    assert response.status_code == 404

def test_delete_cart(cart_data):
    url = f"{API_BASE_URL}/carts/{cart_data['cart_id']}"
    headers = {
        'Authorization': f'Bearer {BEARER_TOKEN}'
    }
    response = requests.delete(url, headers=headers)
    print('Delete Cart Response:', response.status_code)
    assert response.status_code == 204

    # Verify the cart was deleted
    response = requests.get(url, headers=headers)
    assert response.status_code == 404
