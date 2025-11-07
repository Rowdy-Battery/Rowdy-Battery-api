# Rowdy Battery API
**ACCTMIS 4630 — Lab 4: APIs and Data Access**

This project implements the **backend API** for the Rowdy Battery application.  
It demonstrates how to expose data from a database using **ASP.NET Core Web API**, **Entity Framework Core**, and **SQLite**.

---

## 🚀 How to Run the API
1. Open a terminal in the project root (`Rowdy-Battery-Api`).
2. Restore packages and build:
   ```bash
   dotnet restore
   dotnet build
   ```
3. Start the API:
   ```bash
   dotnet run --project RowdyBattery.Api
   ```
4. When the console shows:
   ```
   Now listening on: http://localhost:5007
   ```
   open this in your browser:
   - http://localhost:5007/swagger (port may differ)

---

## 🗂 Project Structure
```
RowdyBattery.sln
 ├── RowdyBattery.Domain        // Item & Rating classes (business logic)
 ├── RowdyBattery.Data          // StoreContext (EF Core DbContext)
 └── RowdyBattery.Api           // Controllers, Swagger, EF registration
```

---

## 🧩 Endpoints
| Verb | Route | Description | Example Status Codes |
|------|--------|-------------|----------------------|
| GET | `/api/Catalog` | Return all catalog items | 200 OK |
| GET | `/api/Catalog/{id}` | Return single item by Id | 200 OK / 404 Not Found |
| POST | `/api/Catalog` | Add new item | 201 Created / 400 Bad Request |
| PUT | `/api/Catalog/{id}` | Update item by Id | 200 OK / 404 Not Found |
| DELETE | `/api/Catalog/{id}` | Delete item by Id | 204 No Content / 404 Not Found |
| POST | `/api/Catalog/{id}/ratings` | Add rating to item | 200 OK / 400 Bad Request / 404 Not Found |

Each route has been tested through **Swagger UI** and the **REST Client** extension.

---

## 🧠 Core Concepts
- **Controllers** expose REST endpoints for the catalog.  
- **Entity Framework Core + SQLite** provides persistent data storage.  
- **Dependency Injection** wires `StoreContext` into controllers.  
- **Swagger UI** documents all routes and response codes.

---

## 🧱 Database Setup (Part 2)
1. Add EF Core packages (if not already present):
   ```bash
   dotnet add RowdyBattery.Api package Microsoft.EntityFrameworkCore.Sqlite
   dotnet add RowdyBattery.Api package Microsoft.EntityFrameworkCore.Design
   dotnet add RowdyBattery.Api package Microsoft.EntityFrameworkCore.Tools
   ```
2. Create the SQLite database and run migrations:
   ```bash
   dotnet ef migrations add init --project RowdyBattery.Api --startup-project RowdyBattery.Api
   dotnet ef database update --project RowdyBattery.Api --startup-project RowdyBattery.Api
   ```
3. Confirm `Registrar.sqlite` appears in the API folder.  
   Add `*.sqlite` to `.gitignore` so the database isn’t pushed to GitHub.

---

## 🧾 Example Requests
**POST /api/Catalog**
```json
{
  "name": "Rowdy Battery 5000 mAh",
  "price": 79.99
}
```

**POST /api/Catalog/1/ratings**
```json
{
  "stars": 5,
  "userName": "John",
  "review": "Excellent battery life!"
}
```

---

## 🔐 CORS (for React frontend)
Allow requests from your React dev server (`http://localhost:3000`):
```csharp
const string AllowFrontend = "AllowFrontend";
builder.Services.AddCors(options =>
    options.AddPolicy(AllowFrontend, p =>
        p.WithOrigins("http://localhost:3000")
         .AllowAnyHeader()
         .AllowAnyMethod()));
...
app.UseCors(AllowFrontend);
```

Or, in the React project’s `package.json`, add:
```json
"proxy": "http://localhost:5007"
```

---

## ✅ Lab Completion Checklist
- [x] CatalogController with all CRUD + Ratings endpoints  
- [x] Entity Framework Core + SQLite integration  
- [x] Database migrations created and updated  
- [x] Swagger UI shows all routes and response codes  
- [x] Tested via Swagger and REST Client  
- [x] `.gitignore` excludes `*.sqlite`  
- [x] Code pushed to GitHub (main branch)

---

© 2025 Rowdy Battery — ACCTMIS 4630 Business Systems Application Development
