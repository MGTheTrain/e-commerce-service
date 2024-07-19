# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.9.0] - 19-07-2024

### Updated

- Modify permissions for `GetAllCarts()` method in [CartController.cs](./backend/Mgtt.ECom/src/Mgtt.ECom.Web/v1/ShoppingCart/Controllers/CartController.cs) component
- Ensure proper navigation handling from the cart list component and within the cart component
- Modify [page not found component](./frontend/e-commerce-service/src/app/components/error-pages/page-not-found/)

## [0.8.0] - 17-07-2024

### Updated

- [Feature] Shopping Cart
  - Cart Functionality
    - [x] Allow users to add products to their cart, view cart contents and update quantities.
  - Cart Persistence
    - [x] Ensure cart contents persist across user sessions.
- Modify the [entity relationship diagram](./docs/diagrams/entity-relationship-diagram.mmd) to ensure that the cart item entity includes the `UserID` property
- Update the [smoke tests](./backend/Mgtt.ECom/smoke-test/) to remove the obsolete user ID from the request DTO and enable smoke tests for the order management in order controller 

## [0.7.0] - 07-07-2024

### Updated

- Modify authorization policies and adjust `Authorize attributes` for HTTP controller endpoints in the [web backend](./backend/Mgtt.ECom/)
- Update [smoke tests](./backend/Mgtt.ECom/smoke-test/) to align with the latest HTTP controller endpoints incorporating bearer tokens.
- Modify [api-design](./docs/api-design/) and [diagrams](./docs/diagrams/)
- Modify [web frontend](./frontend/e-commerce-service/) to align with changes in the web backend incorporating bearer tokens
- Replace [swagger generated Swagger-generated services](./frontend/e-commerce-service/src/app/generated/)

### Removed

- Eliminate user and category domain as it will be managed through the Auth0 platform

## [0.6.0] - 02-07-2024

### Added

- [Feature] User Management
    - User Registration & Authentication
        - [x] Securely handle user sign-up, login, and role management.
        - [x] Communicate with IAM provider APIs like Auth0.
    - Consider OIDC trough `auth0-angular` npm package in web frontend
    - Consider RBAC in web backend trough `Microsoft.AspNetCore.Authentication.JwtBearer` nuget package

## [0.5.0] - 01-07-2024

### Updated

- Integrated CodeQL into [PR](./.github/workflows/pr.yml) and [release](./.github/workflows/release.yml) workflows

### Fixed

- Resolved issues with TypeScript tests

## [0.4.0] - 29-06-2024

### Added

- Setup [Github workflows](./.github/workflows/)
- Setup eslint for [web frontend](./frontend/e-commerce-service/)

### Removed 

- Remove unused directories in [web backend](./backend/Mgtt.ECom/)

## [0.3.0] - 29-06-2024

### Added

- Configure the [web frontend](./frontend/e-commerce-service/) to use Swagger-generated services with `HttpClient` in the components
- Add [order-creation](./frontend/e-commerce-service/src/app/components/order-management/order-creation/), [review-creation](./frontend/e-commerce-service/src/app/components/review-management/review-creation/), [product-creation](./frontend/e-commerce-service/src/app/components/product-management/product-creation/) components 
- Enable CORS in the [web backend](./backend/Mgtt.ECom/)

### Updated

- Updated interfaces, implementations and HTTP endpoints in the [web backend](./backend/Mgtt.ECom/)

### Deleted

- Remove **web frontend using mocked data**

## [0.2.0] - 24-06-2024

### Updated

- Revise the [web frontend using mocked data](./frontend/e-commerce-service-mocked/) to include a header component shared by other components, simulate login/logout and registration processes and improve navigation.

## [0.1.0] - 23-06-2024

### Added

- Established initial project setup, including API design and entity-relationship diagrams.
- Implemented and tested the initial [web backend](./backend/Mgtt.ECom/), adhering to the design.
- Developed the initial [web frontend with mocked data](./frontend/e-commerce-service-mocked/).