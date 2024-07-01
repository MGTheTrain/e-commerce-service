# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.5.0] - 01-07-2024

### Updated

- Integrated CodeQL into the workflow

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