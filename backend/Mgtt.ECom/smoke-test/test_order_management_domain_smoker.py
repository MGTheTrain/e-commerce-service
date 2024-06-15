import requests
import uuid
import pytest
from datetime import datetime
import os

API_BASE_URL = os.getenv('API_BASE_URL', 'http://localhost/api/v1')
TEST_USER_ID = str(uuid.uuid4())  # Sample UUID for test

@pytest.fixture(scope="module")
def order_id():
    url = f"{API_BASE_URL}/orders"
    body = {
        'userID': TEST_USER_ID,
        'orderDate': datetime.utcnow().isoformat(),
        'totalAmount': 100.0,
        'orderStatus': 'Pending'
    }
    response = requests.post(url, json=body)
    assert response.status_code == 201
    return response.json().get('orderID')

@pytest.fixture(scope="module")
def order_item_id(order_id):
    url = f"{API_BASE_URL}/orders/{order_id}/items"
    body = {
        'productID': str(uuid.uuid4()),  # Sample product ID
        'quantity': 1,
        'price': 50.0
    }
    response = requests.post(url, json=body)
    assert response.status_code == 201
    return response.json().get('orderItemID')

def test_create_order():
    url = f"{API_BASE_URL}/orders"
    body = {
        'userID': TEST_USER_ID,
        'orderDate': datetime.utcnow().isoformat(),
        'totalAmount': 100.0,
        'orderStatus': 'Pending'
    }
    response = requests.post(url, json=body)
    print('Create Order Response:', response.status_code)
    assert response.status_code == 201
    assert 'orderID' in response.json()

def test_get_orders():
    url = f"{API_BASE_URL}/orders"
    response = requests.get(url)
    print('Get Orders Response:', response.status_code)
    assert response.status_code == 200

def test_get_order_by_id(order_id):
    url = f"{API_BASE_URL}/orders/{order_id}"
    response = requests.get(url)
    print('Get Order by ID Response:', response.status_code)
    assert response.status_code == 200
    assert response.json().get('orderID') == order_id

def test_update_order(order_id):
    url = f"{API_BASE_URL}/orders/{order_id}"
    body = {
        'userID': TEST_USER_ID,
        'orderDate': datetime.utcnow().isoformat(),
        'totalAmount': 150.0,
        'orderStatus': 'Completed'
    }
    response = requests.put(url, json=body)
    print('Update Order Response:', response.status_code)
    assert response.status_code == 200
    updated_order = response.json()
    assert updated_order.get('totalAmount') == 150.0
    assert updated_order.get('orderStatus') == 'Completed'

def test_delete_order(order_id):
    url = f"{API_BASE_URL}/orders/{order_id}"
    response = requests.delete(url)
    print('Delete Order Response:', response.status_code)
    assert response.status_code == 204

    # Verify the order was deleted
    response = requests.get(url)
    assert response.status_code == 404

def test_create_order_item(order_id):
    url = f"{API_BASE_URL}/orders/{order_id}/items"
    body = {
        'productID': str(uuid.uuid4()),  # Sample product ID
        'quantity': 1,
        'price': 50.0
    }
    response = requests.post(url, json=body)
    print('Create Order Item Response:', response.status_code)
    assert response.status_code == 201
    assert 'orderItemID' in response.json()

def test_get_order_items_by_order_id(order_id):
    url = f"{API_BASE_URL}/orders/{order_id}/items"
    response = requests.get(url)
    print('Get Order Items by Order ID Response:', response.status_code)
    assert response.status_code == 200
    assert isinstance(response.json(), list)

def test_get_order_item_by_id(order_id, order_item_id):
    url = f"{API_BASE_URL}/orders/{order_id}/items/{order_item_id}"
    response = requests.get(url)
    print('Get Order Item by ID Response:', response.status_code)
    assert response.status_code == 200
    assert response.json().get('orderItemID') == order_item_id

def test_update_order_item(order_id, order_item_id):
    url = f"{API_BASE_URL}/orders/{order_id}/items/{order_item_id}"
    body = {
        'productID': str(uuid.uuid4()),  # Sample product ID
        'quantity': 2,
        'price': 100.0
    }
    response = requests.put(url, json=body)
    print('Update Order Item Response:', response.status_code)
    assert response.status_code == 200
    updated_order_item = response.json()
    assert updated_order_item.get('quantity') == 2
    assert updated_order_item.get('price') == 100.0

def test_delete_order_item(order_id, order_item_id):
    url = f"{API_BASE_URL}/orders/{order_id}/items/{order_item_id}"
    response = requests.delete(url)
    print('Delete Order Item Response:', response.status_code)
    assert response.status_code == 204

    # Verify the order item was deleted
    response = requests.get(url)
    assert response.status_code == 404
