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


## 8. ER Diagram

ER diagram using dbdiagram.io:

Table users {
id int [pk, increment]
username varchar
password varchar
role varchar
}

Table tasks {
id int [pk, increment]
title varchar
description text
created_date datetime
due_date datetime
is_completed boolean
user_id int [ref: > users.id]
}

Table task_comments {
id int [pk, increment]
content text
created_date datetime
task_id int [ref: > tasks.id]
user_id int [ref: > users.id]
}

   
## 9. Sample SQL Queries

```sql
-- Get all tasks assigned to a user
SELECT t.* 
FROM Tasks t
WHERE t.UserId = 1;

-- Get all comments on a task
SELECT c.*, u.Username as Commenter
FROM TaskComments c
JOIN Users u ON c.UserId = u.Id
WHERE c.TaskId = 1;