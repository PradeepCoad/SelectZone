# ⚡Select Zone
In this project, we will create an ASP.NET Core web application that allows users to upload files from their local system. The application will save these files to a specified path on the server and store the file paths in a database. This project will demonstrate key concepts such as file handling, database integration, and user input validation in ASP.NET Core.





## ⚡Pre-Requisite: 

` To run an ASP.NET Core project, ensure you have the following: `

#### .NET SDK: Download and install from Microsoft.

#### IDE: Use Visual Studio, Visual Studio Code, or JetBrains Rider.

#### Runtime: Install the ASP.NET Core Runtime.

#### Web Server: Use Kestrel (built-in) or other servers like IIS.
#### Database Server: PostgreSQL 🐘


## ⚡Need to Update : 
#### 1. In Home controller needs to update the "Path" as per your requirement to save files on the server side
```yaml 
string path = Path.Combine("D:\\SelectZone\\Home\\FileMaster\\", username,DateTime.Today.ToString("yyyy-MM-dd"));
```

#### 2. In Home controller needs to update the "connection" string to connect to your PostgreSQL Database
```yaml 
 using (NpgsqlConnection connection = new NpgsqlConnection("Host=localhost/public IP;Username=postgres;Password=Your DB Password;Database=SelectZone")){...}
```
##### Note: Restore the Database File "SelectZone.sql" to your DB to run properly.
