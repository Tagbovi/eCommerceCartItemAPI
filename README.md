# eCommerceCartItemAPI
 Api for eCommenceCart in (Asp.Net)
 
## Overview

This repository contains the source code for an eCommerce CartItem API implemented in ASP.NET. The API supports standard CRUD operations (Create, Read, Update, Delete) for managing cart items.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed (version X.X or later)
- [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/) for development

 ## Packages
 -System.IdentityModels.Tokens.Jwt
 -Micorsoft.FrameWorkCore.Tools
 -Microsoft.AspNetCore.Authentication.JwtBearer
 -FakeItEasy
 -AutoMapper
 -Micorsoft.EntityFrameWorkCore
 -NpgSql.EntityFrameWork.Postgres(database extension of choice for database connectivity)
 
## Getting Started

1. Clone the repository:

    ```bash
    git clone https://github.com/yourusername/eCommerceCartItemAPI.git
    ```

2. Navigate to the project directory:

    ```bash
    cd eCommerceCartItemAPI
    ```

3. Open the solution in your preferred IDE (Visual Studio or Visual Studio Code).

4. Build the solution:

    ```bash
    dotnet build
    ```

5. Run the application:

    ```bash
    dotnet run --project eCommerceCartItemAPI
    ```

The API will be accessible at `https://localhost:7135` by default.

## API Endpoints

- `GET /api/Cart`: Get all cart items.
- `GET /api/Cart/totalItems&totalPrice`: Keep count of the number of cartitems and totalprice of all items.
- `GET /api/Cart/{id}`: Get a specific cart item by ID.
- `POST /api/Cart/`: Create a new cart item.
- `POST /api/CartUserLogin `: Helps Authenticates user for new Jwt token
- `PUT /api/Cart/{id}`: Update an existing cart item.
- `DELETE /api/cartitems/{id}`: Delete a cart item.

## Testing

### Unit Tests

## Prerequisites
- xUnit
- FakeItEasy
This project uses xUnit for unit testing and FakeItEasy for mocking. To run the unit tests, use the following command:

```bash
dotnet test

Or run test from the the ShoppingCart.Test project 





### How the API design considers scalability and concurrency:

# Asynchronous Programming and async/await:
• The use of asynchronous programming the API design, specifically the usage of async and await keywords in method signatures 
  allows application to efficiently handle large number of concurrent requests without blocking threads.

# Task-Based Asynchronous Pattern :
• The use of the Task-Based Asynchronous Pattern (TAP) in the API. TAP simplifies the process of working with asynchronous operations, 
  making it easier to write and maintain asynchronous code.
• TAP allows the API to represent asynchronous operations as tasks, which can be managed and awaited for improved readability and maintainability

