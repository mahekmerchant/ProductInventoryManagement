# ProductInventoryManagement
Running the API
Prerequisites:
Install .NET SDK if you haven’t already.
Ensure you have a SQL Server instance or an in-memory database set up (for testing purposes).
Clone the Repository: Clone your project repository to your local machine.
Configure Database Connection:
Open the appsettings.json file in your project.
Update the DefaultConnection string with your database connection details (if using SQL Server).
Build and Run:
Open a terminal/command prompt in the project directory.
Run the following commands:
dotnet restore
dotnet build
dotnet run

Your API should now be running locally at http://localhost:5000.
API Endpoints:
The API endpoints will be available under the base URL http://localhost:5000/api/products.
You can test them using tools like Postman or your browser.
