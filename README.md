# The Full Stack Team (TFST)

👉 **[Versión en Español](../docs/README-es.md)**

**TFST** is an open-source platform designed to revolutionize **freelance and project management** by integrating **transparency, smart contracts, and decentralized trust mechanisms**. It provides professionals, businesses, and recruiters with the tools they need to manage **clients, projects, contracts, billing, and work hours**—all in a **scalable, multitenant** environment.

## 🌍 Why TFST?

TFST is more than a freelancer marketplace—it’s a **HUB** where top IT talent connects with the best opportunities in a **transparent and efficient** way.

- **For companies** → Access verified IT freelancers without intermediaries.  
- **For IT professionals** → Fair payments, global opportunities, and a **reputation-based growth system**.  
- **For recruiters** → Pre-evaluated technical profiles and **streamlined hiring processes**.  

---

## 🚀 Features

- **🔹 Decentralized Trust** → Blockchain-based smart contracts for payments and reputation.  
- **🔹 Multitenant Support** → Manage multiple clients with **isolated data**.  
- **🔹 Project & Client Management** → Assign professionals to projects and **track progress**.  
- **🔹 Billing & Contracts** → Automated invoicing and **secure agreements**.  
- **🔹 Time Tracking** → Log work hours and **monitor productivity**.  
- **🔹 Transparent Roadmap** → Open development with a **community-driven** approach.  

---

## 🛠️ Tech Stack (Flexible)

TFST is built with **modern, scalable** technologies, but **remains open to improvements** as the platform evolves.

- **Infrastructure & Cloud** → Azure, Terraform, Terragrunt  
- **Frontend** → Angular  
- **Backend** → .NET  
- **Databases** → PostgreSQL or SQL Server (TBD), CosmosDB (Mongo)  
- **Containerization** → Docker  
- **CI/CD & Automation** → Azure DevOps  
- **AI & Blockchain** → Yet to be defined, exploring best-fit solutions  

---

## 📌 Roadmap  

### **MVP (First 3 Months)**  
✅ Freelancer & client **registration** with KYC validation.  
✅ Profile system with **skill-based filtering**.  
✅ Initial hiring and **smart contract-based payments**.  

### **Phase 2 (Next 6 Months)**  
✅ Full **project management** with time tracking & automated payments.  
✅ **Reputation system** based on client validation and technical assessments.  
✅ **Consulting marketplace** for mentorship & training.  

### **Challenges We Are Tackling**  
✅ **Scalability** → Microservices architecture to **support high traffic**.  
✅ **Security** → Smart contract auditing & **data protection**.  
✅ **User Experience** → Simple & intuitive UI/UX for **high conversion rates**.  

---

## ⚡ Installation

## Prerequisites
Ensure you have the following installed:
- **Git**
- **.NET SDK 10.0**
- **Node.js (v18.x) and npm**
- **Angular CLI**
- **Docker**

# Setup Instructions

## 1️⃣ Clone the Repository
```bash
git clone https://github.com/jgccon/tfst.git
cd tfst
```

## 2️⃣ Development Certificate Setup
Before running the solution, you need to generate a development certificate:

```bash
cd src
# Create directory for certificates if it doesn't exist
mkdir -p certs
# Generate self-signed development certificate
dotnet dev-certs https -ep ./certs/tfst_dev_cert.pfx -p Password123*
# Trust the certificate (only needed for local development)
dotnet dev-certs https --trust
```

## 3️⃣ Backend Setup (TFST.API)
The backend solution is in **.NET** and should be run from **Visual Studio**.

1. **Open the solution in Visual Studio**  
   - Open `TFST.sln` using **Visual Studio 2022** (Requires .NET 7+).
   - Select **TFST.API** as the startup project.

2. **Run the backend**  
   - Use `Ctrl + F5` or press `Run` in Visual Studio.

## 4️⃣ Frontend Setup (Clients)
The clients are located in `/src/clients/`, and each one is an independent solution.  

👉 **Each client has its own `README.md` with detailed instructions**.

### How to open the clients:
- Open each client with **Visual Studio Code** or any other editor.
- Client locations:
```
  /src/clients/full-stack-team
  /src/clients/tfst-app
  /src/clients/tfst-dev
```
- To run an Angular client:
```bash
  cd src/clients/tfst-app
  npm install
  ng serve
```

## 5️⃣ Run with Docker (Optional)
If you prefer to run everything using Docker:
```bash
docker-compose up -d
```

## 6️⃣ Run the Application Locally
If not using Docker:
1. **Run the backend from Visual Studio**.
2. **Manually start the frontend**:
```bash
   cd src/clients/tfst-app
   ng serve
```

# CI/CD with Azure DevOps

[ALREADY DONE HERE](https://dev.azure.com/jgcarmona/TheFullStackTeam/)

## Contribution Guidelines
We welcome contributions! Please refer to [CONTRIBUTING.md](CONTRIBUTING.md) for more details.

## Documentation
For detailed documentation, refer to the `/docs` folder. And we will start working on a pair of documentation web sites soon. We'll keep you informed.

## License
Licensed under the MIT License. See [LICENSE](LICENSE) for more details.
