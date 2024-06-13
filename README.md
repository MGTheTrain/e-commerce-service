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

## API Structure

| Management Area       | Services                            | DTOs                                                                | Controllers        | Endpoints                                                                                                         |
|-----------------------|-------------------------------------|---------------------------------------------------------------------|--------------------|------------------------------------------------------------------------------------------------------------------|
| **User Management**   | `IUserService`, `UserService`       | `UserRequestDTO`, `UserResponseDTO`, `LoginRequestDTO`, `LoginResponseDTO` | `UserController`    | `POST /api/users/register`, `POST /api/users/login`, `GET /api/users/{id}`, `PUT /api/users/{id}`, `DELETE /api/users/{id}` |
| **Product Management**| `IProductService`, `ProductService` | `ProductRequestDTO`, `ProductResponseDTO`, `CategoryRequestDTO`, `CategoryResponseDTO` | `ProductController`, `CategoryController` | `POST /api/products`, `GET /api/products`, `GET /api/products/{id}`, `PUT /api/products/{id}`, `DELETE /api/products/{id}`, `POST /api/categories`, `GET /api/categories`, `GET /api/categories/{id}`, `PUT /api/categories/{id}`, `DELETE /api/categories/{id}` |
| **Order Management**  | `IOrderService`, `OrderService`     | `OrderRequestDTO`, `OrderResponseDTO`, `OrderItemRequestDTO`, `OrderItemResponseDTO`    | `OrderController`   | `POST /api/orders`, `GET /api/orders`, `GET /api/orders/{id}`, `PUT /api/orders/{id}`, `DELETE /api/orders/{id}` |
| **Shopping Cart**     | `ICartService`, `CartService`       | `CartRequestDTO`, `CartResponseDTO`, `CartItemRequestDTO`, `CartItemResponseDTO`        | `CartController`    | `POST /api/cart`, `GET /api/cart`, `PUT /api/cart`, `DELETE /api/cart/{id}`                                      |
| **Review Management** | `IReviewService`, `ReviewService`   | `ReviewRequestDTO`, `ReviewResponseDTO`                                                  | `ReviewController`  | `POST /api/reviews`, `GET /api/reviews`, `GET /api/reviews/{id}`, `PUT /api/reviews/{id}`, `DELETE /api/reviews/{id}` |


## Getting Started

TBD
