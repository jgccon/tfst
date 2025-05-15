---
id: 08-crosscutting-concepts
title: Crosscutting Concepts
sidebar_position: 8
---

# 8. Crosscutting Concepts

## Security
- **Authentication**: Managed via JWT and Auth0/Azure AD B2C.
- **Authorization**: Role-based access control for different user levels.

## Logging and Monitoring
- **Logging**: Use Serilog or similar framework for structured logging and Open Telemetry.
- **Monitoring**: Azure Monitor to track application health.

## Configuration Management
- Store configuration settings in Azure App Configuration or environment variables.

## Data Protection & Compliance

### **Regulatory Compliance**
TFST follows the **GDPR (General Data Protection Regulation)** for European users and **CCPA (California Consumer Privacy Act)** for users in the United States.

- **User Rights**:
  - Access to their personal data.
  - Right to request data deletion.
  - Right to restrict processing of their data.

### **Data Security Measures**
- **Data Encryption**: All sensitive data is encrypted at rest and in transit.
- **Token Security**: OAuth2 tokens are short-lived and securely stored.
- **Access Logs**: User access logs are stored securely for auditing.

#### **Data Protection Officer (DPO)**
For any privacy-related concerns or to exercise data rights, users can contact:  
📧 **juan@jgcarmona.com**

