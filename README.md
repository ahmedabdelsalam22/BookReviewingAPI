
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

For detailed documentation on the available endpoints and their functionalities, please refer to the [API documentation](API_DOCUMENTATION.md) file.

## Contribution Guidelines

Contributions to the BookReviewingAPI project are welcome. If you find any issues or have suggestions for improvements, please open an issue or submit a pull request.

When contributing, please ensure the following:

- Follow the existing code style and conventions.
- Write clear and concise commit messages.
- Provide appropriate documentation for new features or changes.

## Contact

If you have any questions or inquiries regarding the BookReviewingAPI project, please contact the repository owner at [ahmedabdelsalam22@gmail.com](mailto:er909112@gmail.com).

Thank you for your interest in the BookReviewingAPI!
