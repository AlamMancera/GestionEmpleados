# Sistema de Gestión de Empleados - Curso POO C#

Este es el proyecto final de la **Sección 4: Encapsulación** del curso [C# Curso completo de Programación Orientada a Objetos .NET](https://www.udemy.com/course/programacion-orientada-a-objetos-con-c-sharp/). El objetivo principal es demostrar el uso correcto de modificadores de acceso, validación de datos a través de propiedades y la separación de responsabilidades.

## 🚀 Características

- **Registro de Empleados:** Validación estricta de campos (RFC, NSS, Email) mediante Expresiones Regulares (Regex).
- **Gestión de Nómina:** Cálculo de salarios, aumentos porcentuales y estadísticas (máximos, mínimos y promedios).
- **Control de Estados:** Sistema para activar o suspender empleados, restringiendo operaciones como aumentos si el empleado está inactivo.
- **Reportes Detallados:** Generación de reportes utilizando `record` para inmutabilidad y strings literales para formato limpio.
- **Seguridad de Datos:** Aplicación de encapsulación para proteger el estado interno de los objetos y listas.

## 🛠️ Conceptos de POO Aplicados

* **Encapsulación:** Uso de campos privados (`_nombre`, `_salario`) con propiedades públicas que validan la entrada de datos.
* **Miembros Estáticos:** Implementación de un `GestorEmpleados` estático para manejar el estado global de la lista sin necesidad de instanciarlo.
* **Inmutabilidad:** Uso de `readonly` para IDs y fechas de contratación, asegurando que no cambien después de la creación.
* **Enumeraciones (`enum`):** Control de estados del empleado (`Activo`, `Inactivo`) y tipos de campos de validación.
* **Separación de Responsabilidades:** División clara entre la lógica de datos (`Empleado`), la lógica de negocio (`GestorEmpleados`) y la interfaz de usuario (`MenuPrincipal`).

## 📦 Estructura del Proyecto

* `Program.cs`: Punto de entrada que inicia el ciclo de vida del menú.
* `Empleado.cs`: Clase base con lógica de validación y comportamiento del objeto.
* `GestorEmpleados.cs`: Clase estática que actúa como base de datos en memoria.
* `MenuPrincipal.cs`: Lógica de interacción con la consola.
* `ReporteEmpleado.cs`: Record utilizado para la transferencia de datos y visualización de reportes.
