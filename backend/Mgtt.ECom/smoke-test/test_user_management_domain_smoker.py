import requests
import uuid
import pytest
import os

API_BASE_URL = os.getenv('API_BASE_URL', 'http://localhost:7260/api')
TEST_USER_NAME = "test_user"
TEST_USER_EMAIL = f"{uuid.uuid4()}@example.com"
TEST_USER_PASSWORD = "test_password"
TEST_USER_ROLE = "user"

@pytest.fixture(scope="module")
def user_id():
    # Register a new user and return the user ID
    url = f"{API_BASE_URL}/users/register"
    body = {
        'userName': TEST_USER_NAME,
        'password': TEST_USER_PASSWORD,
        'email': TEST_USER_EMAIL,
        'role': TEST_USER_ROLE
    }
    response = requests.post(url, json=body, verify=False)  # Set verify=False if using self-signed certificates
    assert response.status_code == 201
    user = response.json()
    return user['userID']

def test_register_user():
    url = f"{API_BASE_URL}/users/register"
    body = {
        'userName': "new_test_user",
        'password': "new_test_password",
        'email': f"{uuid.uuid4()}@example.com",
        'role': "admin"
    }
    response = requests.post(url, json=body, verify=False)
    print('Register User Response:', response.status_code)
    assert response.status_code == 201
    assert 'userID' in response.json()

def test_get_user_by_id(user_id):
    url = f"{API_BASE_URL}/users/{user_id}"
    response = requests.get(url, verify=False)
    print('Get User By ID Response:', response.status_code)
    assert response.status_code == 200
    assert response.json().get('userID') == user_id

def test_update_user(user_id):
    url = f"{API_BASE_URL}/users/{user_id}"
    body = {
        'userName': "updated_test_user",
        'password': "updated_test_password",
        'email': f"{uuid.uuid4()}@example.com",
        'role': "user"
    }
    response = requests.put(url, json=body, verify=False)
    print('Update User Response:', response.status_code)
    assert response.status_code == 200
    updated_user = response.json()
    assert updated_user.get('userName') == "updated_test_user"
    assert updated_user.get('role') == "user"

def test_delete_user(user_id):
    url = f"{API_BASE_URL}/users/{user_id}"
    response = requests.delete(url, verify=False)
    print('Delete User Response:', response.status_code)
    assert response.status_code == 204

    # Verify the user was deleted
    response = requests.get(url, verify=False)
    assert response.status_code == 404
