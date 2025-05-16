---
id: payments-billing
title: Payments Billing
---

# Pagos y Facturación

## Resumen
Gestiona pagos seguros, facturación y transacciones con depósito en garantía.

## Entidades Principales
- **Pago**: Representa una transacción financiera entre un freelancer y un cliente.
- **Factura**: Un documento generado que detalla el trabajo realizado y el pago solicitado.
- **Depósito en garantía**: Un sistema para almacenar fondos de forma segura hasta la finalización del contrato.

## Relaciones
- **Pago por contrato (1:1)**: Los pagos están vinculados a los contratos.
- **Facturas de usuario (1:N)**: Un freelancer puede emitir varias facturas.

```mermaid
erDiagram
Contrato ||--|| Pago: vinculado a
Usuario ||--o{ Factura: emite
Pago ||--o{ Depósito en garantía: asegura
```

## Características Clave
- Pagos seguros con depósito en garantía.
- Facturación automatizada según la finalización del trabajo. - Compatibilidad con múltiples métodos de pago (PayPal, Stripe, Criptomonedas).

## Mejoras futuras
- Depósito en garantía basado en blockchain para pagos transparentes.
- Depósito en garantía multipartito y pagos por hitos.