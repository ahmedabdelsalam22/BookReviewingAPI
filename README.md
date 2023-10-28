
# BookReviewingAPI

<h2>
  Database Design
</h2>

![Screenshot (847)](https://github.com/ahmedabdelsalam22/BookReviewingAPI/assets/75587814/18965f35-e475-42e9-827c-3be05e54ee09)

**BookReviewingAPI**

BookReviewingAPI is a web API project designed to provide functionalities for managing book reviews. This repository contains the source code and associated files for the API.

## Features

- User authentication: Allows users to register and log in to the system.
- Book management: Provides endpoints for creating, retrieving, updating, and deleting books.
- Review management: Enables users to submit reviews for books and retrieve aggregated review data.
- User roles: Supports role-based authorization, including roles such as "admin" and "user".
- API documentation: Offers comprehensive documentation for the available endpoints and their functionalities.

## Technologies Used

- ASP.NET Core: The API is built using the ASP.NET Core framework, which provides a robust and scalable platform for web development.
- Entity Framework Core: Used for data access and database management, Entity Framework Core simplifies interacting with the database.
- SQL Server: The API utilizes SQL Server as the underlying database to store book and user information.
- Authentication and Authorization: The API employs token-based authentication using JSON Web Tokens (JWT) for secure user authentication and role-based authorization.

## Getting Started

To get started with the BookReviewingAPI, follow these steps:

1. Clone the repository:

   ````bash
   git clone https://github.com/ahmedabdelsalam22/BookReviewingAPI.git
   ```

2. Set up the database:
   - Create a new SQL Server database.
   - Update the connection string in the `appsettings.json` file with your database credentials.

3. Build and run the API:
   - Open the solution in Visual Studio or your preferred development environment.
   - Build the solution to restore NuGet packages and compile the project.
   - Run the project, which will start the API server.

4. Explore the API:
   - Open a web browser or an API testing tool (e.g., Postman).
   - Access the API endpoints using the base URL (`http://localhost:port/api/v1/`), where `port` is the port number configured in your development environment.
   - Refer to the API documentation or the source code for details on the available endpoints and their usage.

## API Documentation

### AuthController
Sure! Based on the code provided in the AuthController.cs file, here are the endpoints of the API controller along with their descriptions:

1. **Login**
   - Endpoint: POST /api/v1/login
   - Description: Authenticates a user and generates an authentication token.
   - Request Body: LoginRequestDTO object containing the username and password.
   - Response: Returns an APIResponse object containing the authentication token if the login is successful. Otherwise, returns a 400 Bad Request with an error message.

2. **Register**
   - Endpoint: POST /api/v1/register
   - Description: Registers a new user.
   - Request Body: RegisterRequestDTO object containing the user details such as username, password, etc.
   - Response: Returns an APIResponse object indicating the success or failure of the registration process. If successful, the response will contain a success message. Otherwise, returns a 400 Bad Request with an error message.

3. **AssignRole**
   - Endpoint: POST /api/v1/assignRole
   - Description: Assigns a role to a user.
   - Request Parameters:
     - email: The email address of the user.
     - roleName: The role to assign to the user.
   - Response: Returns an APIResponse object indicating the success or failure of the role assignment process. If successful, the response will contain the assigned role. Otherwise, returns a 400 Bad Request with an error message.

Please note that these endpoints are based on the code provided and may not cover all possible scenarios or validations. It's recommended to review the code implementation and perform additional testing as needed.


### AuthorController

Certainly! Here are the documentation for the endpoints of the `AuthorController.cs` in the BookReviewingAPI:

## API Endpoints

### Get all authors
- **URL:** `/api/v1/allAuthors`
- **Method:** GET
- **Description:** Retrieves a list of all authors.
- **Headers:**
  - Authorization: Bearer [access_token]
- **Response:**
  - Status Code: 200 (OK)
  - Body:
    ```json
    {
      "statusCode": 200,
      "isSuccess": true,
      "result": [
        {
          "id": 1,
          "name": "John Doe",
          ...
        },
        ...
      ]
    }
    ```

### Get author by ID
- **URL:** `/api/v1/author/{id}`
- **Method:** GET
- **Description:** Retrieves a specific author by their ID.
- **Headers:**
  - Authorization: Bearer [access_token]
- **Parameters:**
  - id: The ID of the author.
- **Response:**
  - Status Code: 200 (OK)
  - Body:
    ```json
    {
      "statusCode": 200,
      "isSuccess": true,
      "result": {
        "id": 1,
        "name": "John Doe",
        ...
      }
    }
    ```

### Get authors by book ID
- **URL:** `/api/v1/{bookId}/authors`
- **Method:** GET
- **Description:** Retrieves a list of authors associated with a specific book.
- **Headers:**
  - Authorization: Bearer [access_token]
- **Parameters:**
  - bookId: The ID of the book.
- **Response:**
  - Status Code: 200 (OK)
  - Body:
    ```json
    {
      "statusCode": 200,
      "isSuccess": true,
      "result": [
        {
          "id": 1,
          "name": "John Doe",
          ...
        },
        ...
      ]
    }
    ```

### Get books by author ID
- **URL:** `/api/v1/{authorId}/books`
- **Method:** GET
- **Description:** Retrieves a list of books written by a specific author.
- **Headers:**
  - Authorization: Bearer [access_token]
- **Parameters:**
  - authorId: The ID of the author.
- **Response:**
  - Status Code: 200 (OK)
  - Body:
    ```json
    {
      "statusCode": 200,
      "isSuccess": true,
      "result": [
        {
          "id": 1,
          "title": "Book Title",
          ...
        },
        ...
      ]
    }
    ```

### Create new author
- **URL:** `/api/v1/authors/create`
- **Method:** POST
- **Description:** Creates a new author.
- **Headers:**
  - Authorization: Bearer [access_token]
- **Request Body:**
  ````json
  {
    "name": "John Doe",
    ...
  }
  ```
- **Response:**
  - Status Code: 201 (Created)
  - Body:
    ```json
    {
      "statusCode": 201,
      "isSuccess": true,
      "result": {
        "id": 1,
        "name": "John Doe",
        ...
      }
    }
    ```

Please note that the endpoints require authentication using a bearer token in the `Authorization` header.

# Book Controller 

Endpoint Documentation for BookController.cs

This document provides an overview of the endpoints available in the `BookController.cs` file of the BookReviewingAPI project. Please note that the documentation covers only the endpoints present in the provided code snippet.

## API Version

The API version used for the endpoints in this controller is `v1.0`.

### Get All Books

Endpoint: `GET api/v1.0/allBooks`

- Description: Retrieves all books from the database.
- Authorization: Requires authentication.
- Response:
  - Status 200 OK: Returns a list of books in the response body.
  - Status 400 Bad Request: Indicates a problem with the request.
  - Status 404 Not Found: If no books exist in the database.

### Get Book by ID

Endpoint: `GET api/v1.0/book/{bookId}`

- Description: Retrieves a specific book by its ID.
- Authorization: Requires authentication.
- Parameters:
  - `bookId` (integer): The ID of the book to retrieve.
- Response:
  - Status 200 OK: Returns the book details in the response body.
  - Status 400 Bad Request: Indicates a problem with the request.
  - Status 404 Not Found: If no book exists with the specified ID.

### Get Book by ISBN

Endpoint: `GET api/v1.0/book/isbn/{isbn}`

- Description: Retrieves a specific book by its ISBN.
- Authorization: Requires authentication.
- Parameters:
  - `isbn` (string): The ISBN of the book to retrieve.
- Response:
  - Status 200 OK: Returns the book details in the response body.
  - Status 400 Bad Request: Indicates a problem with the request.
  - Status 404 Not Found: If no book exists with the specified ISBN.

### Get Rating by Book ID

Endpoint: `GET api/v1.0/book/{bookId}/rating`

- Description: Retrieves the ratings of a book by its ID.
- Authorization: Requires authentication.
- Parameters:
  - `bookId` (integer): The ID of the book to retrieve ratings for.
- Response:
  - Status 200 OK: Returns a list of ratings in the response body.
  - Status 404 Not Found: If no book exists with the specified ID.

### Create Book

Endpoint: `POST api/v1.0/book/create`

- Description: Creates a new book.
- Authorization: Requires authentication with `admin` role.
- Request Body: Expects a JSON object representing the book to be created. The structure should follow the `BookCreateDTO` model.
- Response:
  - Status 200 OK: Returns the created book details in the response body.
  - Status 400 Bad Request: Indicates a problem with the request, such as missing or invalid data.

### Update Book

Endpoint: `PUT api/v1.0/book/{bookId}`

- Description: Updates an existing book by its ID.
- Authorization: Requires authentication with `admin` role.
- Parameters:
  - `bookId` (integer): The ID of the book to update.
- Request Body: Expects a JSON object representing the updated book data. The structure should follow the `BookUpdateDTO` model.
- Response:
  - Status 200 OK: Returns the updated book details in the response body.
  - Status 400 Bad Request: Indicates a problem with the request, such as missing or invalid data.
  - Status 404 Not Found: If no book exists with the specified ID.
  - Status 304 Not Modified: If the book data is not modified.

Please note that the documentation provided here is based solely on the information available in the provided code snippet. There may be additional endpoints or functionality in the complete codebase that are not covered in this document.

# Category Controller 

Sure! Based on the provided code, here are the descriptions of the endpoints in the `CategoryController` of the BookReviewingAPI:

1. **GET /api/v1/allCategories**

   - Description: Retrieves all categories.
   - Authorization: Required.
   - Response:
     - 200 OK: Returns a list of `CategoryDTO` objects representing all categories.
     - 404 Not Found: If no categories are found.
     - 400 Bad Request: If an error occurs during the process.

2. **GET /api/v1/category/{categoryId}**

   - Description: Retrieves a specific category by its ID.
   - Authorization: Required.
   - Parameters:
     - categoryId: The ID of the category to retrieve.
   - Response:
     - 200 OK: Returns a `CategoryDTO` object representing the category.
     - 404 Not Found: If the category with the specified ID is not found.
     - 400 Bad Request: If an error occurs during the process.

3. **GET /api/v1/categories/bookId/{bookId}**

   - Description: Retrieves all categories related to a specific book.
   - Authorization: Required.
   - Parameters:
     - bookId: The ID of the book.
   - Response:
     - 200 OK: Returns a list of `CategoryDTO` objects representing the categories related to the book.
     - 404 Not Found: If the book with the specified ID is not found or no categories are associated with the book.
     - 400 Bad Request: If an error occurs during the process.

4. **GET /api/v1/books/categoryId/{categoryId}**

   - Description: Retrieves all books belonging to a specific category.
   - Authorization: Required.
   - Parameters:
     - categoryId: The ID of the category.
   - Response:
     - 200 OK: Returns a list of `BookDTO` objects representing the books belonging to the category.
     - 404 Not Found: If the category with the specified ID is not found or no books are associated with the category.
     - 400 Bad Request: If an error occurs during the process.

5. **POST /api/v1/category/create**

   - Description: Creates a new category.
   - Authorization: Required (admin role).
   - Request Body: Expects a JSON object representing the `CategoryDTO` with the following properties:
     - Name: The name of the category.
   - Response:
     - 200 OK: Returns the newly created `Category` object.
     - 404 Not Found: If the category already exists.
     - 400 Bad Request: If the request body is invalid or an error occurs during the process.

Please note that these descriptions are based solely on the provided code snippet, and there may be additional details or validations implemented in other parts of the application. It's always recommended to refer to the complete documentation or consult the developer for a comprehensive understanding of the API's functionality and usage.


# Country Controller

Sure! Here are the documentation for the endpoints of the `CountryController` in the provided API controller file:

## CountryController

This controller handles operations related to countries.

### Retrieve all countries

Retrieves a list of all countries.

```
[GET] /api/v1/country/allCountries
```

**Response:**

- 200 OK: Returns a list of `CountryDTO` objects representing the countries.
- 400 Bad Request: If there was an error in the request.
- 404 Not Found: If no countries exist.

### Retrieve a country by ID

Retrieves a single country based on its ID.

```
[GET] /api/v1/country/{countryId}
```

- Replace `{countryId}` with the ID of the country.

**Response:**

- 200 OK: Returns a `CountryDTO` object representing the country.
- 400 Bad Request: If there was an error in the request.
- 404 Not Found: If no country exists with the specified ID.

### Retrieve the country of an author

Retrieves the country associated with a specific author.

```
[GET] /api/v1/{authorId}/country
```

- Replace `{authorId}` with the ID of the author.

**Response:**

- 200 OK: Returns a `Country` object representing the country associated with the author.
- 400 Bad Request: If there was an error in the request.
- 404 Not Found: If no author exists with the specified ID or if the author has no associated country.

### Retrieve authors by country ID

Retrieves a list of authors belonging to a specific country.

```
[GET] /api/v1/{countryId}/authors
```

- Replace `{countryId}` with the ID of the country.

**Response:**

- 200 OK: Returns a list of `Author` objects representing the authors belonging to the country.
- 400 Bad Request: If there was an error in the request.
- 404 Not Found: If no country exists with the specified ID or if there are no authors associated with the country.

### Create a new country

Creates a new country.

```
[POST] /api/v1/create
```

**Request Body:**

The request body should contain the details of the country to be created in the following format:

```json
{
  "name": "Country Name",
  "population": 1000000
}
```

- Replace `"Country Name"` with the name of the country.
- Replace `1000000` with the population of the country.

**Response:**

- 200 OK: Returns a success message indicating that the country was created successfully.
- 400 Bad Request: If there was an error in the request or the request body is invalid.

Please note that some endpoints require authorization. Make sure to include the necessary authentication headers when making requests to those endpoints.

# Review Controller

Sure! Based on the provided code snippet from the `ReviewController.cs` file, I will provide documentation for the endpoints in the API controller.

## ReviewController

### Retrieve All Reviews
Retrieves all reviews.

**Endpoint:** `GET /api/v1/allReviews`

**Response:** Returns a list of reviews or an error message.

### Retrieve Review by ID
Retrieves a review based on its ID.

**Endpoint:** `GET /api/v1/review/{id}`

**Parameters:**
- `id` (integer): The ID of the review to retrieve.

**Response:** Returns the review with the specified ID or an error message.

### Retrieve Reviews by Book ID
Retrieves all reviews associated with a specific book.

**Endpoint:** `GET /api/v1/reviews/bookId/{bookId}`

**Parameters:**
- `bookId` (integer): The ID of the book.

**Response:** Returns a list of reviews associated with the specified book or an error message.

### Retrieve Book by Review ID
Retrieves the book associated with a specific review.

**Endpoint:** `GET /api/v1/book/reviewId/{reviewId}`

**Parameters:**
- `reviewId` (integer): The ID of the review.

**Response:** Returns the book associated with the specified review or an error message.

### Create Review
Creates a new review.

**Endpoint:** `POST /api/v1/review/create`

**Request Body:** The request body should contain the details of the review to create.

**Response:** Returns the created review or an error message.

Please note that the endpoints in the documentation above are relative to the base URL of the API, and the version number `v1` is included in the URL path. You may need to adjust the base URL and version number according to your API setup.

Remember to include appropriate authorization and authentication mechanisms to secure your API endpoints as per your application requirements.

# Reviewer Controller 

Sure! Based on the code provided in the `ReviewerController.cs` file, here are the documentation for the endpoints:

## Reviewer Controller

### Get All Reviewers

Returns a list of all reviewers.

- **URL:** `/api/v1/allReviewers`
- **Method:** GET
- **Authorization:** Required
- **Response Body:**

```json
{
  "statusCode": 200,
  "isSuccess": true,
  "result": [
    {
      "id": 1,
      "name": "John Doe",
      "email": "johndoe@example.com"
      // Additional reviewer properties
    },
    // Additional reviewers
  ]
}
```

### Get Reviewer by ID

Returns a specific reviewer based on the provided ID.

- **URL:** `/api/v1/reviewer/{id}`
- **Method:** GET
- **Authorization:** Required
- **Parameters:**
  - `{id}` (integer, required) - The ID of the reviewer.
- **Response Body:**

```json
{
  "statusCode": 200,
  "isSuccess": true,
  "result": {
    "id": 1,
    "name": "John Doe",
    "email": "johndoe@example.com"
    // Additional reviewer properties
  }
}
```

### Get Reviews of Reviewer

Returns a list of reviews associated with a specific reviewer based on the provided reviewer ID.

- **URL:** `/api/v1/reviewer/{reviewerId}/reviews`
- **Method:** GET
- **Authorization:** Required
- **Parameters:**
  - `{reviewerId}` (integer, required) - The ID of the reviewer.
- **Response Body:**

```json
{
  "statusCode": 200,
  "isSuccess": true,
  "result": [
    {
      "id": 1,
      "title": "Review Title",
      "content": "Review content"
      // Additional review properties
    },
    // Additional reviews
  ]
}
```

### Get Reviewer by Review ID

Returns the reviewer associated with a specific review based on the provided review ID.

- **URL:** `/api/v1/reviewers/{reviewId}/reviewer`
- **Method:** GET
- **Authorization:** Required
- **Parameters:**
  - `{reviewId}` (integer, required) - The ID of the review.
- **Response Body:**

```json
{
  "statusCode": 200,
  "isSuccess": true,
  "result": {
    "id": 1,
    "name": "John Doe",
    "email": "johndoe@example.com"
    // Additional reviewer properties
  }
}
```

### Create Reviewer

Creates a new reviewer.

- **URL:** `/api/v1/reviewer/create`
- **Method:** POST
- **Authorization:** Required (Role: admin)
- **Request Body:**

```json
{
  "name": "John Doe",
  "email": "johndoe@example.com"
  // Additional reviewer properties
}
```

- **Response Body:**

```json
{
  "statusCode": 200,
  "isSuccess": true,
  "result": {
    "id": 1,
    "name": "John Doe",
    "email": "johndoe@example.com"
    // Additional reviewer properties
  }
}
```

Please note that the documentation provided above is based on the code snippet you provided. Make sure to review the actual implementation in the `ReviewerController.cs` file for complete accuracy.

## Contribution Guidelines

Contributions to the BookReviewingAPI project are welcome. If you find any issues or have suggestions for improvements, please open an issue or submit a pull request.

When contributing, please ensure the following:

- Follow the existing code style and conventions.
- Write clear and concise commit messages.
- Provide appropriate documentation for new features or changes.

## Contact

If you have any questions or inquiries regarding the BookReviewingAPI project, please contact the repository owner at [ahmedabdelsalam22@gmail.com](mailto:er909112@gmail.com).

Thank you for your interest in the BookReviewingAPI!
