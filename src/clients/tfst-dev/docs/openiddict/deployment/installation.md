---
id: installation
title: Installation
---

# Installation and Deployment of TFST.AuthServer, TFST.API and tfst-demo

This document provides step by step instructions for installing and deploying TFST.AuthServer, TFST.API and tfst-demo projects.

## Prerequisites

Before you begin with the installation, ensure you have the following prerequisites:

- .NET SDK (specific version)
- Database (SQL Server)

## Installation Steps

### 1. Clone the Repository

Clone the GitHub repository containing the projects:

```bash
git clone https://github.com/jgccon/tfst.git
cd tfst
```

### 2. Configure the Database

Create a database in your database management system and configure the connection strings in the corresponding configuration files.

### 3. Install Dependencies

For each project, navigate to the project folder and run the following command to install the dependencies:

```bash
cd TFST.AuthServer
dotnet restore

cd ../TFST.API
dotnet restore
```

### 4. Configure Environment Variables

Make sure to configure the necessary environment variables for each project. This may include connection strings, secret keys, and other configuration parameters.

### 5. Run Migrations

Run the database migrations to initialize the database structure:

```bash
cd TFST.AuthServer
dotnet ef database update
```

### 6. Start the Projects

Start each of the projects in their respective folders:

```bash
cd TFST.AuthServer
dotnet run

cd ../TFST.API
dotnet run

cd ../tfst-demo
dotnet run
```

## Verification

Once all projects are running, verify that you can access the following URLs:

- TFST.API: `https://localhost:5001`
- TFST.AuthServer: `https://localhost:6001`
- tfst-demo: `http://localhost:7000`
