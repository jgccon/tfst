---
id: 04-solution-strategy
title: Estrategia de Solución
sidebar_position: 4
---

# 4. Estrategia de la solución

## Enfoque arquitectónico
- **Multiinquilino**: Implementado a nivel de base de datos para dar soporte a múltiples clientes con datos aislados.
- **CI/CD**: Utilizar GitHub Actions para automatizar las pruebas, la compilación y la implementación.
- **Administración de secretos**: Utilizar Azure Key Vault para almacenar información confidencial.
- **Contenedorización**: Utilizar Docker para entornos de desarrollo e implementación consistentes.