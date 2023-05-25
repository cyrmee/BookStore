## Installation and Setup

These instructions will guide you through the installation and setup process for the project.

### Prerequisites

- Docker
- Docker Compose

### Step 1: Clone the Repository

Clone the project repository to your local machine using the following command:

```bash
git clone <repository-url>
```

### Step 2: Set Environment Variables

Copy the contents of the following code block and create a new file named `.env` in the root directory of the project. Replace the placeholder values with your own:

```plaintext
POSTGRES_USER=  # Replace with the username for the PostgreSQL database
POSTGRES_PASSWORD=  # Replace with the password for the PostgreSQL database
POSTGRES_DB=  # Replace with the name of the PostgreSQL database
```

After creating the `.env` file, update the connection string in the `appsettings.json` file located in the `Presentation` directory. Set the value of the "host" parameter in the connection string to the container name of the PostgreSQL database.

### Step 3: Build and Run the Containers

Open a terminal or command prompt, navigate to the project's root directory, and run the following command to build and run the Docker containers:

```bash
docker-compose up -d --build
```

This command will start the containers defined in the `docker-compose.yml` file. The `-d` flag runs the containers in detached mode, allowing them to continue running in the background.

### Step 4: Access the Application

Once the containers are up and running, you can access the application in your web browser at `http://localhost:5000`. If everything is set up correctly, you should see the application's interface.

### Additional Notes

- The PostgreSQL database container will be accessible at `localhost:5432`, and the default Redis container will be accessible at `localhost:6379`.
- The Nginx container will be running on port 80 and forwarding requests to the web API container.

## Contributing

Explain how others can contribute to the project, such as by submitting bug reports or pull requests.

## License

MIT License
