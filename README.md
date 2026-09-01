# Hospital Management System API

## Overview

The Hospital Management System (HMS) API is a backend application built to manage and streamline core hospital operations.

The system provides centralized management for doctors, patients, appointments, and medical records while maintaining a scalable and well-structured architecture.

The project follows Clean Architecture principles to ensure separation of concerns, maintainability, scalability, and testability.

---

## Features

### Doctor Management

* Add, update, and delete doctors
* View doctor profiles and information
* Assign doctors to departments
* Manage doctor availability

### Patient Management

* Register new patients
* Update patient information
* View patient profiles
* Access patient medical history

### Appointment Management

* Book appointments
* Cancel appointments
* Reschedule appointments
* Track appointment status
* Manage appointments between doctors and patients

### Medical Records

* Create and manage patient medical records
* Store diagnoses
* Add prescriptions
* Track treatments
* View patient medical history

### Authentication & Authorization

* JWT-based authentication
* Secure API endpoints
* Role-based authorization
* Supported roles:

  * Admin
  * Doctor
  * Patient

---

## Architecture

The project follows Clean Architecture to keep the business logic independent from external frameworks and infrastructure.

### API Layer

Responsible for handling HTTP requests and responses.

Includes:

* Controllers
* API Endpoints
* Middleware
* Authentication configuration
* Dependency Injection configuration

### Application Layer

Contains the application business logic and use cases.

Includes:

* Services
* DTOs
* Interfaces
* Validation
* Business rules

### Domain Layer

Contains the core business entities and domain definitions.

Includes:

* Entities
* Enums
* Domain interfaces
* Core business rules

### Infrastructure Layer

Responsible for external services and data persistence.

Includes:

* Entity Framework Core
* SQL Server
* DbContext
* Repository implementations
* Database configurations

---

## Technologies Used

* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* LINQ
* JWT Authentication
* Swagger / OpenAPI
* Dependency Injection
* Clean Architecture

---

## Project Structure

```text
HospitalManagementSystem/
│
├── HospitalManagementSystem.API/
│   ├── Controllers/
│   ├── Middleware/
│   ├── Extensions/
│   ├── Program.cs
│   └── appsettings.json
│
├── HospitalManagementSystem.Application/
│   ├── DTOs/
│   ├── Interfaces/
│   ├── Services/
│   ├── Validators/
│   └── UseCases/
│
├── HospitalManagementSystem.Domain/
│   ├── Entities/
│   ├── Enums/
│   └── Interfaces/
│
├── HospitalManagementSystem.Infrastructure/
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   └── Configurations/
│   ├── Repositories/
│   └── Migrations/
│
└── HospitalManagementSystem.sln
```

---

## Main Entities

The system is built around the following core entities:

* Doctor
* Patient
* Department
* Appointment
* MedicalRecord
* Prescription
* User
* Role

---

## API Documentation

The API is documented using Swagger / OpenAPI.

Swagger provides an interactive interface that allows developers to:

* Explore available endpoints
* View request and response models
* Test API endpoints
* Send authenticated requests using JWT tokens

---

## Security

The API uses JWT Authentication to secure protected resources.

Authorization is based on user roles to ensure that each user can only access the operations permitted for their role.

For example:

* Admins can manage doctors, patients, departments, and system resources.
* Doctors can manage appointments and patient medical records.
* Patients can manage their appointments and view their own medical information.

---

## Database

SQL Server is used as the primary relational database.

Entity Framework Core is used as the ORM for:

* Database operations
* Entity relationships
* LINQ queries
* Database migrations
* Data persistence

---

## Project Goals

The main goals of this project are:

* Apply Clean Architecture principles
* Build a maintainable and scalable RESTful API
* Implement secure authentication and authorization
* Apply Entity Framework Core best practices
* Maintain clear separation between application layers
* Provide organized hospital data management
