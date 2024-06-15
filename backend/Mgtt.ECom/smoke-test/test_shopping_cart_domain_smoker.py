import requests
import uuid
import pytest

API_BASE_URL = 'http://localhost/api/v1'
TEST_USER_ID = str(uuid.uuid4())  # Sample UUID for test

@pytest.fixture(scope="module")
def cart_id():
    # Setup: Create a cart and return its ID
    url = f"{API_BASE_URL}/carts"
    body = {'userID': TEST_USER_ID, 'totalAmount': 100.0}
    response = requests.post(url, json=body)
    assert response.status_code == 201
    return response.json().get('cartID')

def test_get_cart(cart_id):
    url = f"{API_BASE_URL}/carts/{cart_id}"
    response = requests.get(url)
    print('Get Cart Response:', response.status_code)
    assert response.status_code == 200

def test_update_cart(cart_id):
    url = f"{API_BASE_URL}/carts/{cart_id}"
    body = {'totalAmount': 150.0}
    response = requests.put(url, json=body)
    print('Update Cart Response:', response.status_code)
    assert response.status_code == 200

def test_add_item_to_cart(cart_id):
    url = f"{API_BASE_URL}/carts/{cart_id}/items"
    body = {
        'productID': str(uuid.uuid4()),  # Sample product ID
        'quantity': 1,
        'price': 50.0
    }
    response = requests.post(url, json=body)
    print('Add Item to Cart Response:', response.status_code)
    assert response.status_code == 200

def test_delete_cart(cart_id):
    url = f"{API_BASE_URL}/carts/{cart_id}"
    response = requests.delete(url)
    print('Delete Cart Response:', response.status_code)
    assert response.status_code == 204
