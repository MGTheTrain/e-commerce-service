# e-commerce-service

## Table of Contents

- [Summary](#summary)
- [Features](#features)
- [API Structure](#api-structure)
- [Getting Started](#getting-started)

## Summary

Simple front- and backend for an e-commerce platform utilizing DDD principles based on an [existing cookiecutter template](https://github.com/MGTheTrain/dotnet-ddd-web-api-starter)

## Features

| **Domain**             | **Feature**                                | **Description**                                                             |
|------------------------|--------------------------------------------|-----------------------------------------------------------------------------|
| **User Management**    | User Registration & Authentication         | Securely handle user sign-up, login, and role management.                   |
|                        | User Profile Management                    | Manage user details such as username, email, and password.                  |
| **Product Management** | Product Catalog                            | Create, read, update, and delete (CRUD) operations for products.            |
|                        | Category Management                        | Organize products into categories for easier navigation and searchability.  |
| **Order Management**   | Order Processing                           | Manage customer orders from creation to completion.                         |
|                        | Order Items                                | Handle individual items within an order, including quantity and price details. |
| **Shopping Cart**      | Cart Functionality                         | Allow users to add products to their cart, view cart contents, and update quantities. |
|                        | Cart Persistence                           | Ensure cart contents persist across user sessions.                          |
| **Review Management**  | Product Reviews                            | Enable users to write reviews for products, rate them, and provide feedback. |
|                        | Review Moderation                          | Manage and moderate user reviews to maintain quality and trustworthiness.   |

**NOTE:** Check out the [entity relationship diagram](./docs/diagrams/domain-model-schema.mmd)

## API Structure

| Domain                | Interfaces & Services                                                                 | DTOs                                                                                 | Controllers        | Endpoints                                                                                                         |
|-----------------------|----------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------|--------------------|------------------------------------------------------------------------------------------------------------------|
| **User Management**   | `IUserService`, `UserService`                                                          | `UserRequestDTO`, `UserResponseDTO`, `LoginRequestDTO`, `LoginResponseDTO`           | `UserController`   | `POST /api/users/register`, `POST /api/users/login`, `GET /api/users/{userId}`, `PUT /api/users/{userId}`, `DELETE /api/users/{userId}` |
| **Product Management**| `IProductService`, `ProductService`, `ICategoryService`, `CategoryService`             | `ProductRequestDTO`, `ProductResponseDTO`, `CategoryRequestDTO`, `CategoryResponseDTO` | `ProductController`, `CategoryController` | `POST /api/products`, `GET /api/products`, `GET /api/products/{productId}`, `PUT /api/products/{productId}`, `DELETE /api/products/{productId}`, `POST /api/categories`, `GET /api/categories`, `GET /api/categories/{categoryId}`, `PUT /api/categories/{categoryId}`, `DELETE /api/categories/{categoryId}` |
| **Order Management**  | `IOrderService`, `OrderService`, `IOrderItemService`, `OrderItemService`               | `OrderRequestDTO`, `OrderResponseDTO`, `OrderItemRequestDTO`, `OrderItemResponseDTO` | `OrderController`  | `POST /api/orders`, `GET /api/orders`, `GET /api/orders/{orderId}`, `PUT /api/orders/{orderId}`, `DELETE /api/orders/{orderId}`, `POST /api/orders/{orderId}/items`, `GET /api/orders/{orderId}/items`, `GET /api/orders/{orderId}/items/{itemId}`, `PUT /api/orders/{orderId}/items/{itemId}`, `DELETE /api/orders/{orderId}/items/{itemId}` |
| **Shopping Cart**     | `ICartService`, `CartService`, `ICartItemService`, `CartItemService`                   | `CartRequestDTO`, `CartResponseDTO`, `CartItemRequestDTO`, `CartItemResponseDTO`     | `CartController`   | `POST /api/carts`, `GET /api/carts/{cartId}`, `PUT /api/carts/{cartId}`, `DELETE /api/carts/{cartId}`, `POST /api/carts/{cartId}/items`, `GET /api/carts/{cartId}/items`, `GET /api/carts/{cartId}/items/{itemId}`, `PUT /api/carts/{cartId}/items/{itemId}`, `DELETE /api/cart/{cartId}/items/{itemId}`                                      |
| **Review Management** | `IReviewService`, `ReviewService`                                                      | `ReviewRequestDTO`, `ReviewResponseDTO`                                              | `ReviewController` | `POST /api/reviews`, `GET /api/reviews`, `GET /api/reviews/{reviewId}`, `PUT /api/reviews/{reviewId}`, `DELETE /api/reviews/{reviewId}` |

## Getting Started

TBD
