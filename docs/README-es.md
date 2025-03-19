# The Full Stack Team (TFST)

**TFST** es una plataforma de código abierto diseñada para revolucionar la **gestión de proyectos y freelance** mediante la integración de **transparencia, contratos inteligentes y mecanismos de confianza descentralizados**. Proporciona a los profesionales, empresas y reclutadores las herramientas que necesitan para gestionar **clientes, proyectos, contratos, facturación y horas de trabajo**, todo en un entorno **escalable y multiusuario**.

## 🌍 ¿Por qué TFST?

TFST es más que un mercado de freelancers: es un **CENTRO** donde los mejores talentos de TI se conectan con las mejores oportunidades de una manera **transparente y eficiente**.

- **Para empresas** → Accede a freelancers de TI verificados sin intermediarios.
- **Para profesionales de TI** → Pagos justos, oportunidades globales y un **sistema de crecimiento basado en la reputación**.
- **Para reclutadores** → Perfiles técnicos preevaluados y **procesos de contratación optimizados**.

---

## 🚀 Características

- **🔹 Confianza descentralizada** → Contratos inteligentes basados ​​en blockchain para pagos y reputación.
- **🔹 Compatibilidad con múltiples inquilinos** → Administra múltiples clientes con **datos aislados**.
- **🔹 Gestión de proyectos y clientes** → Asigna profesionales a proyectos y **sigue el progreso**.
- **🔹 Facturación y contratos** → Facturación automatizada y **acuerdos seguros**.
- **🔹 Seguimiento del tiempo** → Registra las horas de trabajo y **monitorea la productividad**.
- **🔹 Hoja de ruta transparente** → Desarrollo abierto con un enfoque **impulsado por la comunidad**.

---

## 🛠️ Pila tecnológica (flexible)

TFST está construida con tecnologías **modernas y escalables**, pero **permanece abierta a mejoras** a medida que la plataforma evoluciona.

- **Infraestructura y nube** → Azure, Terraform, Terragrunt
- **Frontend** → Angular
- **Backend** → .NET
- **Bases de datos** → PostgreSQL o SQL Server (por determinar), CosmosDB (Mongo)
- **Containerización** → Docker
- **CI/CD y automatización** → Azure DevOps
- **IA y blockchain** → Aún por definir, se están explorando las soluciones más adecuadas

---

## 📌 Hoja de ruta

### **MVP (primeros 3 meses)**
✅ **Registro** de autónomos y clientes con validación KYC.
✅ Sistema de perfiles con **filtrado basado en habilidades**.
✅ Contratación inicial y **pagos basados ​​en contratos inteligentes**.

### **Fase 2 (próximos 6 meses)**
✅ **Gestión de proyectos** completa con seguimiento del tiempo y pagos automáticos.
✅ **Sistema de reputación** basado en la validación del cliente y evaluaciones técnicas.
✅ **Mercado de consultoría** para tutoría y capacitación.

### **Desafíos que estamos abordando**
✅ **Escalabilidad** → Arquitectura de microservicios para **soportar alto tráfico**.
✅ **Seguridad** → Auditoría de contratos inteligentes y **protección de datos**.
✅ **Experiencia del usuario** → UI/UX simple e intuitiva para **altas tasas de conversión**.

---

## ⚡ Instalación

## Requisitos previos
Asegúrese de tener instalado lo siguiente:
- **Git**
- **.NET SDK 8.0**
- **Node.js (v18.x) y npm**
- **Angular CLI**
- **Docker (opcional)**

# Instrucciones de Configuración

## 1️⃣ Clonar el Repositorio
```bash
git clone https://github.com/jgccon/tfst.git
cd tfst
```

## 2️⃣ Configuración del Certificado de Desarrollo
Antes de ejecutar la solución, es necesario generar un certificado de desarrollo:

```bash
cd src
# Crear directorio para certificados si no existe
mkdir -p certs
# Generar certificado auto-firmado para desarrollo
dotnet dev-certs https -ep ./certs/tfst_dev_cert.pfx -p Password123*
# Confiar en el certificado (solo en local)
dotnet dev-certs https --trust
```

## 3️⃣ Configuración del Backend (TFST.API)
El backend de TFST es una solución en **.NET** y debe ejecutarse desde **Visual Studio**.

1. **Abrir la solución en Visual Studio**  
   - Abre `TFST.sln` con **Visual Studio 2022** (Requiere .NET 7+).
   - Selecciona **TFST.API** como proyecto de inicio.

2. **Ejecutar el backend**  
   - Usa `Ctrl + F5` o ejecuta desde el botón `Run` en Visual Studio.

## 4️⃣ Configuración del Frontend (Clientes)
Los clientes están en `/src/clients/` y cada uno es una solución independiente.  

👉 **Cada cliente tiene su propio `README.md` con instrucciones detalladas**.

### Cómo abrir los clientes:
- Puedes abrir cada cliente desde **Visual Studio Code** u otro editor.
- Ubicación de los clientes:
```
  /src/clients/full-stack-team
  /src/clients/tfst-app
  /src/clients/tfst-dev
```
- Para ejecutar un cliente en Angular:
```bash
  cd src/clients/tfst-app
  npm install
  ng serve
```

## 5️⃣ Ejecutar con Docker (Opcional)
Si prefieres ejecutar todo con Docker:
```bash
docker-compose up -d
```

## 6️⃣ Ejecutar la Aplicación Localmente
Si no usas Docker:
1. **Ejecuta el backend desde Visual Studio**.
2. **Ejecuta el frontend manualmente**:
```bash
   cd src/clients/tfst-app
   ng serve
```

# CI/CD con Azure DevOps

[YA REALIZADO AQUI](https://dev.azure.com/jgcarmona/TheFullStackTeam/)

## Guía de Contribución
¡Damos la bienvenida a contribuciones! Consulta [CONTRIBUTING-es.md](CONTRIBUTING-es.md) para más detalles.

## Documentación
Para documentación detallada, consulta la carpeta `/docs` o visita nuestra [página de documentación](docs/README.md).

## Licencia
Licenciado bajo la Licencia MIT. Consulta [LICENSE](LICENSE) para más detalles.
