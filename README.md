# BlazorGrpc

BlazorGrpc is a sample project demonstrating how to implement a gRPC service using ASP.NET Core Blazor and Entity Framework Core for CRUD operations. 

This project uses SQLite as the database provider.

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

Configure the Database Context

In Program.cs, add the necessary configuration for the SQLite database:

using Microsoft.EntityFrameworkCore;
using BlazorGrpc.Data;
using BlazorGrpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ProductContext") ?? "Data Source=products.db"));
builder.Services.AddScoped<ServerProductService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ProductService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();

```
Blazor QuickGrid

<div class="grid" tabindex="-1" style="display: @(loading ? "none" : "block")">
    <QuickGrid Items="@ProductIQueryable" Pagination="@pagination">
        <PropertyColumn Property="@(p => p.Id)"  Sortable="true" />
        <PropertyColumn Property="@(p => p.Name)" Sortable="true" />
        <PropertyColumn Property="@(p => p.Price)" Sortable="true" />
    </QuickGrid>
</div>
