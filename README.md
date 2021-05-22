# P10 OpenClassrooms
A medical solution to Insert, View, Edit and Calculate data about Patients

## USE
[Visual Studio ( or VSCode)](https://visualstudio.microsoft.com/)

With Visual Studio open & Docker running :
```
1. Download the project ( git clone https://github.com/H97-Git/q24uBYLBA1VjJuMAQ4eok.git Path )
1. Open the project ( *.sln )
2. Make sure 'docker-compose(.yml)' is set a start-up project. 
3. CTRL + F5
```
## Patient
Microservice to Create, Edit and Get Patient data (SQL Server) :
- Docker
- ASP.NET Core Web API
- MS SQL Server
- Serilog
- Swagger
## PatientNotes/History
Microservice to Create, Edit and Get PatientNotes/History data : 
- Docker
- ASP.NET Core Web API
- MongoDb
- Swagger
- Serilog
## DiabetesRiskLevel
Microservice to calculate the diabetes risk level of a patient. With datas from Patient & PatientNotes/History
- Docker
- ASP.NET Core Web API
- Swagger
- Serilog
## BlazorPatient
Simple .NET Core Blazor application connected to all services above.
To ship something deliverable.

## Docker Links (when containerized)
- Blazor : [http://localhost:6000/](http://localhost:6000/)
- Patient : [http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)
- Notes : [http://localhost:5002/swagger/index.html](http://localhost:5002/swagger/index.html)
- Assessment : [http://localhost:5004/swagger/index.html](http://localhost:5004/swagger/index.html)
