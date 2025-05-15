---
id: 11-risks-and-technical-debt
title: Risks and Technical Debt
sidebar_position: 11
---

#11. Riesgos y deuda técnica

## Riesgos potenciales
- **Vulnerabilidades de seguridad**: Riesgo de fuga de datos entre inquilinos.
- **Dependencia de un proveedor de nube**: La fuerte dependencia de Azure podría limitar la portabilidad futura.

## Deuda técnica
- **Escalado de soluciones multiinquilino**: Posible necesidad de optimizar la base de datos a medida que aumenta el número de inquilinos.
- **Límites de velocidad de API**: Considere la posibilidad de limitar la velocidad y usar mecanismos de almacenamiento en caché para gestionar el alto tráfico.