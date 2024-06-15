import requests
import uuid
import pytest
import os

API_BASE_URL = os.getenv('API_BASE_URL', 'http://localhost/api/v1')
TEST_PRODUCT_ID = str(uuid.uuid4())  # Replace with an actual product ID from your setup
TEST_USER_ID = str(uuid.uuid4())  # Replace with an actual user ID from your setup
TEST_REVIEW_RATING = 5
TEST_REVIEW_COMMENT = "This is a test review."

def test_create_review():
    url = f"{API_BASE_URL}/reviews"
    body = {
        'productID': TEST_PRODUCT_ID,
        'userID': TEST_USER_ID,
        'rating': 4,
        'comment': "This is another test review."
    }
    response = requests.post(url, json=body)
    print('Create Review Response:', response.status_code)
    assert response.status_code == 201
    assert 'reviewID' in response.json()

def test_get_review_by_id(review_id):
    url = f"{API_BASE_URL}/reviews/{review_id}"
    response = requests.get(url)
    print('Get Review By ID Response:', response.status_code)
    assert response.status_code == 200
    assert response.json().get('reviewID') == review_id

def test_get_reviews_by_product_id():
    url = f"{API_BASE_URL}/reviews/product/{TEST_PRODUCT_ID}"
    response = requests.get(url)
    print('Get Reviews By Product ID Response:', response.status_code)
    assert response.status_code == 200
    assert isinstance(response.json(), list)

def test_get_reviews_by_user_id():
    url = f"{API_BASE_URL}/reviews/user/{TEST_USER_ID}"
    response = requests.get(url)
    print('Get Reviews By User ID Response:', response.status_code)
    assert response.status_code == 200
    assert isinstance(response.json(), list)

def test_update_review(review_id):
    url = f"{API_BASE_URL}/reviews/{review_id}"
    body = {
        'productID': TEST_PRODUCT_ID,
        'userID': TEST_USER_ID,
        'rating': 3,
        'comment': "This is an updated test review."
    }
    response = requests.put(url, json=body)
    print('Update Review Response:', response.status_code)
    assert response.status_code == 200
    updated_review = response.json()
    assert updated_review.get('rating') == 3
    assert updated_review.get('comment') == "This is an updated test review."

def test_delete_review(review_id):
    url = f"{API_BASE_URL}/reviews/{review_id}"
    response = requests.delete(url)
    print('Delete Review Response:', response.status_code)
    assert response.status_code == 204

    # Verify the review was deleted
    response = requests.get(url)
    assert response.status_code == 404
