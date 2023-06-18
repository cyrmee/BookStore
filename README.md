***Installation***

**Prerequisites:**
Make sure you have the following installed on your system:
- Docker: [https://docs.docker.com/get-docker/](https://docs.docker.com/get-docker/)

**Step 1: Clone the Repository**
```shell
git clone https://github.com/cyrmee/BookStore
```

**Step 2: Set Up Environment Variables**
1. Copy `.env.example` to `.env`
```shell
cp .env.example .env
``` 
2. Replace the placeholder values in the `.env` file with your own values:
   - `POSTGRES_USER`: Replace with the username for the PostgreSQL database.
   - `POSTGRES_PASSWORD`: Replace with the password for the PostgreSQL database.
   - `POSTGRES_DB`: Replace with the name of the PostgreSQL database.

**Step 3: Update Connection String**
1. In the root directory of the cloned repository, navigate to the `Presentation` folder.
2. Open `appsettings.json`.
3. Find the connection string and update the `"host"` value with the container name of the PostgreSQL database. The container name should be the same as specified in the `docker-compose.yml` file.

Here's an example of how the updated connection string might look like:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=postgres_container_name;Port=5432;Database=bookstore_db;User Id=your_username;Password=your_password;"
}
```

Make sure to replace "postgres_container_name" with the actual container name of your PostgreSQL database found in `.env` file, "your_username" with the appropriate username, and "your_password" with the corresponding password.

Save the changes to the `appsettings.json` file after making the necessary modifications.

**Step 4: Build and Run the Docker Containers**
1. Open your terminal or command prompt.
2. Change the current working directory to the root directory of the project.
3. Run the following command to build and run the Docker containers:

```shell
docker compose up --build -d
```

This command will build and start the containers defined in the `docker-compose.yml` file.

**Step 5: Access the Application**
- To access the application through Nginx, open your web browser and visit `http://localhost`.
- To access the API directly, open your web browser and visit `http://localhost:5000`. The API is served by the `webapi` container.

That's it! You have successfully installed and set up the project.
