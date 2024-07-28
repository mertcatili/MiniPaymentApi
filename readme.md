## MiniPaymentApi

MiniPaymentApi is a .NET Core-based payment API designed with Domain-Driven Design (DDD) architecture. It includes Swagger for API documentation and logging for tracking purposes. The API supports Pay, Cancel, Refund, and Report operations.

## Features

Payment Operations: Pay, Cancel, Refund, and Report functionalities.
Domain-Driven Design: Clean and maintainable architecture.
Swagger Documentation: Auto-generated API documentation.
Logging: Comprehensive logging for monitoring and debugging.

## Clone the repository:

```bash
$ git clone https://github.com/mertcatili/MiniPaymentApi.git
$ cd MiniPaymentApi
```

## Set up environment variables:
Create a .env file in the root directory with the following content:

```bash
$ AuthToken=MINIPAYMENTAPI_AUTH_TOKEN
```

## Running the Application

```bash
$ dotnet run
```

## Accessing Swagger
Swagger UI will be available at http://localhost:5079/swagger/index.html.

