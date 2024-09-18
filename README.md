
# Banking Project

This project is a **Banking** system that handles user authentication, transactions, and other core banking functionalities.

## Prerequisites

- **Git**: Ensure you have Git installed on your system to clone the repository.
- **Docker**: The project is containerized using Docker. Make sure you have Docker and Docker Compose installed.
- **Visual Studio**: The project uses Visual Studio (VS) for development and debugging.

## Getting Started

To get the project running, follow these steps:

1. **Clone the repository**  
   Open your terminal and run the following command to clone the repository:
   ```bash
   git clone https://github.com/clu9er/Banking.git
   ```

2. **Install Docker**  
   Make sure you have Docker installed on your machine. You can download Docker from [here](https://www.docker.com/get-started).

3. **Open the Project in Visual Studio**  
   Once you have cloned the repository, open the project folder in Visual Studio.

4. **Select Docker Compose as the Startup Project**  
   In Visual Studio, select the **docker-compose** project from the Solution Explorer and set it as the startup project.

5. **Run the Project**  
   Press `F5` or click on the "Run" button to start the project. This will build and run the Docker containers for the Banking project.

## Running Tests

To run the tests for the Banking project:

1. **Open the Test Explorer**  
   In Visual Studio, navigate to `Test` -> `Test Explorer` from the top menu.

2. **Run All Tests**  
   In the Test Explorer window, click "Run All" to execute all the unit and integration tests available for the project.

## Additional Information

- The project utilizes the following technologies:
  - .NET 8
  - PostgreSQL
  - Redis
  - Docker for containerization
  - Visual Studio as the development environment
  - xUnit for testing
  