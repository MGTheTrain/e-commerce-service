import requests
import uuid
import pytest
import os

API_BASE_URL = os.getenv('API_BASE_URL', 'http://localhost:7260/api/v1')  # Adjust port if necessary
TEST_USER_ID = str(uuid.uuid4())  # Sample UUID for test

@pytest.fixture(scope="module")
def cart_data():
    # Setup: Create a cart and return its data (user ID and cart ID)
    url = f"{API_BASE_URL}/carts"
    body = {'userID': TEST_USER_ID, 'totalAmount': 100.0}
    response = requests.post(url, json=body, verify=False)  # Set verify=False if using self-signed certificates
    assert response.status_code == 201
    cart_id = response.json().get('cartID')
    return {'user_id': TEST_USER_ID, 'cart_id': cart_id}

@pytest.fixture(scope="module")
def cart_item_id(cart_data):
    # Setup: Create a cart item and return its ID
    url = f"{API_BASE_URL}/carts/{cart_data['cart_id']}/items"
    body = {
        'productID': str(uuid.uuid4()),  # Sample product ID
        'quantity': 1,
        'price': 50.0
    }
    response = requests.post(url, json=body, verify=False)
    assert response.status_code == 201
    return response.json().get('cartItemID')

def test_create_cart():
    url = f"{API_BASE_URL}/carts"
    body = {'userID': TEST_USER_ID, 'totalAmount': 100.0}
    response = requests.post(url, json=body, verify=False)
    assert response.status_code == 201
    assert 'cartID' in response.json()

def test_get_cart(cart_data):
    url = f"{API_BASE_URL}/carts/{cart_data['cart_id']}"
    response = requests.get(url, verify=False)
    print('Get Cart Response:', response.status_code)
    assert response.status_code == 200

def test_update_cart(cart_data):
    url = f"{API_BASE_URL}/carts/{cart_data['cart_id']}"
    body = {'totalAmount': 150.0}
    response = requests.put(url, json=body, verify=False)
    print('Update Cart Response:', response.status_code)
    assert response.status_code == 200

def test_add_item_to_cart(cart_data):
    url = f"{API_BASE_URL}/carts/{cart_data['cart_id']}/items"
    body = {
        'productID': str(uuid.uuid4()),  # Sample product ID
        'quantity': 1,
        'price': 50.0
    }
    response = requests.post(url, json=body, verify=False)
    print('Add Item to Cart Response:', response.status_code)
    assert response.status_code == 201

def test_get_cart_items(cart_data):
    url = f"{API_BASE_URL}/carts/{cart_data['cart_id']}/items"
    response = requests.get(url, verify=False)
    print('Get Cart Items Response:', response.status_code)
    assert response.status_code == 200

def test_get_cart_item_by_id(cart_data, cart_item_id):
    url = f"{API_BASE_URL}/carts/{cart_data['cart_id']}/items/{cart_item_id}"
    response = requests.get(url, verify=False)
    print('Get Cart Item by ID Response:', response.status_code)
    assert response.status_code == 200

def test_update_cart_item(cart_data, cart_item_id):
    url = f"{API_BASE_URL}/carts/{cart_data['cart_id']}/items/{cart_item_id}"
    body = {
        'productID': str(uuid.uuid4()),  # Sample product ID
        'quantity': 2,
        'price': 100.0
    }
    response = requests.put(url, json=body, verify=False)
    print('Update Cart Item Response:', response.status_code)
    assert response.status_code == 200

def test_delete_cart_item(cart_data, cart_item_id):
    url = f"{API_BASE_URL}/carts/{cart_data['cart_id']}/items/{cart_item_id}"
    response = requests.delete(url, verify=False)
    print('Delete Cart Item Response:', response.status_code)
    assert response.status_code == 204

    # Verify the cart item was deleted
    response = requests.get(url, verify=False)
    assert response.status_code == 404

def test_delete_cart(cart_data):
    url = f"{API_BASE_URL}/carts/{cart_data['cart_id']}"
    response = requests.delete(url, verify=False)
    print('Delete Cart Response:', response.status_code)
    assert response.status_code == 204

    # Verify the cart was deleted
    response = requests.get(url, verify=False)
    assert response.status_code == 404
