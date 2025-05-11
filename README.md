# Task Management API

A simple .NET Core Web API for managing tasks with JWT authentication.

## Features
- Create and manage tasks
- Assign tasks to users
- Add comments to tasks
- JWT authentication with role-based authorization

## Prerequisites
- .NET 7 SDK
- SQL Server Express (or Docker for containerized SQL Server)
- (Optional) Docker for containerization

## Setup
1. Clone the repository
2. Update connection string in `appsettings.json` if needed
3. Run migrations:
   ```bash
   dotnet ef database update