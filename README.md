# e-commerce-service

## Table of Contents

- [Summary](#summary)
- [Features](#features)
- [API Structure](#api-structure)
- [Getting Started](#getting-started)

## Summary

Web front- and backend for an e-commerce platform utilizing DDD principles based on an [existing cookiecutter template](https://github.com/MGTheTrain/dotnet-ddd-web-api-starter) for [the web backend](./backend/Mgtt.ECom/)

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

**NOTE:** Also consider checking out the [entity relationship diagram](./docs/diagrams/entity-relationship-diagram.mmd)

## API Structure

| Domain                | Interfaces & Services                                                                 | DTOs                                                                                 | Controllers        | Endpoints                                                                                                         |
|-----------------------|----------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------|--------------------|------------------------------------------------------------------------------------------------------------------|
| **User Management**   | `IUserService`, `UserService`                                                          | `UserRequestDTO`, `UserResponseDTO`, `LoginRequestDTO`, `LoginResponseDTO`           | `UserController`   | `POST /api/v1/users/register`, `POST /api/v1/users/login`, `GET /api/v1/users/{userId}`, `PUT /api/v1/users/{userId}`, `DELETE /api/v1/users/{userId}` |
| **Product Management**| `IProductService`, `ProductService`, `ICategoryService`, `CategoryService`             | `ProductRequestDTO`, `ProductResponseDTO`, `CategoryRequestDTO`, `CategoryResponseDTO` | `ProductController`, `CategoryController` | `POST /api/v1/products`, `GET /api/v1/products`, `GET /api/v1/products/{productId}`, `PUT /api/v1/products/{productId}`, `DELETE /api/v1/products/{productId}`, `POST /api/v1/categories`, `GET /api/v1/categories`, `GET /api/v1/categories/{categoryId}`, `PUT /api/v1/categories/{categoryId}`, `DELETE /api/v1/categories/{categoryId}` |
| **Order Management**  | `IOrderService`, `OrderService`, `IOrderItemService`, `OrderItemService`               | `OrderRequestDTO`, `OrderResponseDTO`, `OrderItemRequestDTO`, `OrderItemResponseDTO` | `OrderController`  | `POST /api/v1/orders`, `GET /api/v1/orders`, `GET /api/v1/orders/{orderId}`, `PUT /api/v1/orders/{orderId}`, `DELETE /api/v1/orders/{orderId}`, `POST /api/v1/orders/{orderId}/items`, `GET /api/v1/orders/{orderId}/items`, `GET /api/v1/orders/{orderId}/items/{itemId}`, `PUT /api/v1/orders/{orderId}/items/{itemId}`, `DELETE /api/v1/orders/{orderId}/items/{itemId}` |
| **Shopping Cart**     | `ICartService`, `CartService`, `ICartItemService`, `CartItemService`                   | `CartRequestDTO`, `CartResponseDTO`, `CartItemRequestDTO`, `CartItemResponseDTO`     | `CartController`   | `POST /api/v1/carts`, `GET /api/v1/carts/{cartId}`, `PUT /api/v1/carts/{cartId}`, `DELETE /api/v1/carts/{cartId}`, `POST /api/v1/carts/{cartId}/items`, `GET /api/v1/carts/{cartId}/items`, `GET /api/v1/carts/{cartId}/items/{itemId}`, `PUT /api/v1/carts/{cartId}/items/{itemId}`, `DELETE /api/v1/cart/{cartId}/items/{itemId}`                                      |
| **Review Management** | `IReviewService`, `ReviewService`                                                      | `ReviewRequestDTO`, `ReviewResponseDTO`                                              | `ReviewController` | `POST /api/v1/reviews`, `GET /api/v1/reviews`, `GET /api/v1/reviews/{reviewId}`, `PUT /api/v1/reviews/{reviewId}`, `DELETE /api/v1/reviews/{reviewId}` |

Copy contents of the [swagger.json](./docs/api-design/swagger.json) to the [Swagger Online editor](https://editor.swagger.io/).
Results should resemble following snippet:

![swagger-ui-results.PNG](./docs/api-design/swagger-ui-results.PNG)

## Getting Started

### Preconditions

- [Install Docker Engine](https://docs.docker.com/engine/install/)

### Backend

You can find instructions on applicable commands for the backend source code in the following [README.md](./backend/Mgtt.ECom/README.md)

### Frontend

You can find instructions on applicable commands for the frontend source code in the following [README.md](./frontend/e-commerce-service/README.md)

### Local docker compose setup

You can start the web front-end and back-end using the command:

```sh
docker compose up -d --build .
``` 

To view the web backend, open a browser and go to `localhost:5000/swagger/index.html`. Results should resemble:

![Swagger UI trough Docker](./docs/api-design/swagger-ui-trough-docker.PNG)

To view the web frontend, open a browser and go to `localhost:4200`. Results should resemble:

![Web frontend including mock data](./docs/test/web-frontend-including-mock-data.PNG)

You can remove all Docker resources with:

```sh
docker compose down -v
```