# e-commerce-service

## Table of Contents

- [Summary](#summary)
- [Features](#features)
- [Project structure](#project-structure)
- [Getting Started](#getting-started)

## Summary

Simple frontend and backend for an e-commerce platform utilizing DDD principles based on an [existing cookiecutter template](https://github.com/MGTheTrain/dotnet-ddd-web-api-starter)

## Features

### User Management

- User Registration & Authentication: Securely handle user sign-up, login, and role management.
- User Profile Management: Manage user details such as username, email, and password.

### Product Management

- Product Catalog: Create, read, update, and delete (CRUD) operations for products.
- Category Management: Organize products into categories for easier navigation and searchability.

### Order Management

- Order Processing: Manage customer orders from creation to completion.
- Order Items: Handle individual items within an order, including quantity and price details.

### Shopping Cart

- Cart Functionality: Allow users to add products to their cart, view cart contents, and update quantities.
- Cart Persistence: Ensure cart contents persist across user sessions.

### Review Management

- Product Reviews: Enable users to write reviews for products, rate them, and provide feedback.
- Review Moderation: Manage and moderate user reviews to maintain quality and trustworthiness.

## Project Structure

The project is divided into several bounded contexts, each responsible for a specific domain of the application:

- User Management Context: Handles all user-related operations.
- Product Management Context: Manages product details and categorization.
- Order Management Context: Oversees order creation, processing, and management.
- Shopping Cart Context: Manages user shopping carts and their contents.
- Review Management Context: Handles product reviews and ratings.

## Getting Started

TBD
