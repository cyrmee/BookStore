# BookStore API

BookStore API is a RESTful web service that provides functionality for managing books, categories, orders, and user information. It serves as the backend for a bookstore application, allowing users to browse and purchase books online.

## Technologies Used

- Programming Language: C# with ASP.NET Core
- Database: Entity Framework Core with PostgreSQL
- Authentication and Authorization: JSON Web Tokens (JWT)
- Real-time Communication: SignalR
- API Documentation: Swagger/OpenAPI
- Containerization: Docker

## Features

The API offers the following features:

- Book Management: Create, retrieve, update, and delete books.
- Category Management: Manage book categories.
- Order Management: Place new orders, retrieve orders, and manage order details.
- User Management: Authenticate users and authorize access to protected resources.
- Real-time Updates: Utilize SignalR for real-time communication and updates.
- API Documentation: Explore API endpoints and test them using Swagger UI.

##Installation

To install and set up the project, please follow the steps below:

## Prerequisites
Make sure you have the following installed on your system:
- Docker: https://www.docker.com/get-started

## Step 1: Clone the Repository
1. Open your terminal or command prompt.
2. Change the current working directory to the location where you want to clone the repository.
3. Run the following command to clone the repository:

```shell
git clone https://github.com/cyrmee/BookStore
```

## Step 2: Set Up Environment Variables
1. In the root directory of the cloned repository, create a new file named `.env`.
2. Open the `.env` file in a text editor.
3. Copy the contents of the docker-compose.yml file provided and paste them into the `.env` file.
4. Replace the placeholder values in the `.env` file with your own values:
   - `POSTGRES_USER`: Replace with the username for the PostgreSQL database.
   - `POSTGRES_PASSWORD`: Replace with the password for the PostgreSQL database.
   - `POSTGRES_DB`: Replace with the name of the PostgreSQL database.

## Step 3: Update Connection String
1. In the root directory of the cloned repository, navigate to the `Presentation` folder.
2. Open the `appsettings.json` file in a text editor.
3. Find the connection string and update the `"host"` value with the container name of the PostgreSQL database. The container name should be the same as specified in the `docker-compose.yml` file.

## Step 4: Build and Run the Docker Containers
1. Open your terminal or command prompt.
2. Change the current working directory to the root directory of the cloned repository.
3. Run the following command to build and run the Docker containers:

```shell
docker compose up --build -d
```

This command will build and start the containers defined in the `docker-compose.yml` file.

### Step 5: Access the Application
- To access the application through Nginx, open your web browser and visit `http://localhost`.
- To access the API directly, open your web browser and visit `http://localhost:5000`. The API is served by the `webapi` container.

## Folder Structure
Here is an overview of the project's folder structure:

- `Application`: Contains the application-specific code.
- `Domain`: Contains the domain models, repositories, and types.
- `Infrastructure`: Contains the infrastructure-related code, such as configurations, data access, and migrations.
- `Presentation`: Contains the presentation layer code, including controllers, middlewares, and configurations.

That's it! You have successfully installed and set up the project.
