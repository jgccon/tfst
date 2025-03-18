# The Full Stack Team (TFST)

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

### Prerequisites
Ensure you have the following installed:
- **Git**
- **.NET SDK 10.0**
- **Node.js (v18.x) and npm**
- **Angular CLI**
- **Docker**

### Steps
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/jgccon/tfst.git
   cd tfst
   ```

2. **Development Certificate Setup**:
   ```bash
   cd src
   # Create directory for certificates if it doesn't exist
   mkdir -p certs
   # Generate self-signed development certificate
   dotnet dev-certs https -ep ./certs/tfst_dev_cert.pfx -p Password123*
   # Trust the certificate (only needed for local development)
   dotnet dev-certs https --trust
   ```

3. **Backend Setup**:
   ```bash
   cd src/backend/TFST.API
   dotnet build
   ```

4. **Frontend Setup**:
   ```bash
   cd ../webapp
   npm install
   ng serve
   ```

5. **Run with Docker**:
   ```bash
   docker-compose up -d
   ```
   
6. **Run the Application Locally** (Alternative to Docker):
   ```bash
   dotnet run --project src/backend/TFST.API
   ng serve --project webapp
   ```

# CI/CD with Azure DevOps

[ALREADY DONE HERE](https://dev.azure.com/jgcarmona/TheFullStackTeam/)

## Contribution Guidelines
We welcome contributions! Please refer to [CONTRIBUTING.md](CONTRIBUTING.md) for more details.

## Documentation
For detailed documentation, refer to the `/docs` folder or visit our [documentation page](docs/README.md).

## License
Licensed under the MIT License. See [LICENSE](LICENSE) for more details.
