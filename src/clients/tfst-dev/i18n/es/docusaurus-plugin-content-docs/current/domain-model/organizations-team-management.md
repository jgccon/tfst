---
id: organizations-team-management
title: Organizations Team Management
---

# Organizaciones y Gestión de Equipos

## Resumen
Apoya a empresas y agencias en la gestión de sus equipos, la publicación de ofertas de empleo y la adquisición de talento.

## Entidades Principales
- **Organización**: Representa a una empresa o agencia que contrata freelancers.
- **Membresía de la Organización del Usuario**: Un vínculo entre usuarios y organizaciones.
- **Reclutador**: Un rol de usuario especializado dentro de una organización.

## Relaciones
- **Organizaciones del Usuario (0:N)**: Un usuario puede pertenecer a varias organizaciones.
- **Publicados de Empleo de la Organización (1:N)**: Las empresas publican varios empleos.

```mermaid
erDiagram
Usuario ||--o{ Organización: pertenece a
Organización ||--o{ Puesto de Trabajo: crea
Organización ||--o{ Membresía de la Organización del Usuario: gestiona
Membresía de la Organización del Usuario ||--|| Usuario: vincula
```

## Características Clave
- Gestión de organizaciones multiusuario. - Acceso basado en roles para reclutadores y equipos de RR. HH.
- Panel centralizado para el seguimiento de ofertas de empleo y contrataciones.

## Mejoras futuras
- Información de contratación basada en IA y recomendaciones para el equipo.