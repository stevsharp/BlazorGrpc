# BlazorGrpc

BlazorGrpc is a sample project demonstrating how to implement a gRPC service using ASP.NET Core and Entity Framework Core for CRUD operations. This project uses SQLite as the database provider.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- An IDE (e.g., Visual Studio or Visual Studio Code)
- A database (SQLite)

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/stevsharp/BlazorGrpc.git
cd BlazorGrpc

## Setting Up the Project
Install Required Packages

Ensure all necessary packages are installed:

dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Grpc.AspNetCore
