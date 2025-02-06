**Pension Contribution Management System**

  **Project Overview**

The Pension Contribution Management System is a .NET Core 7+ application designed to manage pension contributions, calculate benefits, and handle transactions for members and employers. It follows Clean Architecture principles with Entity Framework Core, Hangfire for background jobs, and SQL Server as the database.


---


1️⃣ Prerequisites

Ensure you have the following installed on your system:

.NET Core 7+ (Download)

SQL Server (Download)

Visual Studio

Swagger (For API testing)


---

2️⃣ Clone the Repository

git clone https://github.com/ToLuWaNiMe/PensionManagement
cd pension-contribution-management


---

3️⃣ Configure Environment Variables

Create an appsettings.json file in the src/API folder with the following:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PensionDB;User Id=sa;Password=YourStrong!Passw0rd;"
  },
  "Hangfire": {
    "DashboardEnabled": true
  }
}

> Replace YourStrong!Passw0rd with a strong database password.




---

4️⃣ Run Database Migrations

Run the following command to apply migrations:

dotnet ef database update --project src/Infrastructure


---

5️⃣ Run the Application

To start the API, navigate to the API project folder and run:

cd src/API
dotnet run

The API will be available at http://localhost:5000 (or https://localhost:5001 for HTTPS).


---

6️⃣ Access Swagger API Documentation

Once the application is running, open:
➡️ http://localhost:5000/swagger
Swagger provides interactive API documentation where you can test endpoints directly.


---

7️⃣ Running Background Jobs with Hangfire

Hangfire is configured for processing contributions and benefit calculations.
Access the Hangfire Dashboard at:
➡️ http://localhost:5000/hangfire

You can see running,and scheduled.


---

8️⃣ Running Unit & Integration Tests

Run unit tests with:

dotnet test

To run integration tests:

dotnet test --filter Category=Integration


---

 API Endpoints
 

🔹 Members

🔹 Contributions

🔹 Benefits


---

**Contribution Guidelines**

1. Fork the repository


2. Create a new branch (feature-xyz)


3. Commit your changes (git commit -m "Added feature XYZ")


4. Push to your branch (git push origin feature-xyz)


5. Open a Pull Request




---

📧 Contact

For any issues, open a GitHub issue or contact:
📩 oladosu.toluene@gmail.com


---

