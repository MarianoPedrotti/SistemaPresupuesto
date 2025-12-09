* Sistema Presupuesto

Sistema Presupuesto es una aplicación de escritorio desarrollada en C# (WinForms) que permite gestionar el ciclo completo de un presupuesto: clientes, productos, encabezados, detalles y confirmaciones.
El proyecto prioriza la integridad de datos, utilizando transacciones SQL, procedimientos almacenados y una arquitectura en capas totalmente independiente (N-Tier).

* Descripción General

La aplicación permite:

Administrar Clientes, Productos, Presupuestos y Detalles de Presupuesto.

Ejecutar operaciones CRUD completas en cada entidad.

Confirmar presupuestos con lógica de negocio asociada.

Garantizar la consistencia del sistema mediante el uso de transacciones SQL, asegurando que el encabezado y todos sus detalles se guarden como una única operación atómica.

El diseño del proyecto está orientado a mantener un código limpio, escalable y fácil de mantener.

* Arquitectura en Capas (N-Tier)
  


El proyecto se estructura en capas totalmente independientes:

1. Presentación (WinForms)

Interfaz de usuario basada en Windows Forms. Incluye las ventanas principales del sistema:

Home / Ventana Principal

Gestión de Cliente (CRUD)

Gestión de Producto (CRUD)

Gestión de Presupuesto

CRUD completo

Validaciones y lógica de negocio

Gestión de Consulta de Detalle:

Consulta enriquecida de líneas de presupuesto (prueba de mapeo de datos de la capa de Business Logic).

2. Business Logic (Reglas de Negocio)

Contiene los servicios que implementan la lógica del sistema, tales como:

Validación de datos

Reglas para crear y actualizar presupuestos

Confirmación de presupuestos

Manejo del estado del presupuesto

Coordinación entre entidades y repositorios

3. Data Access (Acceso a Datos)

Maneja la comunicación directa con la base de datos SQL Server utilizando:

ADO.NET (SqlConnection, SqlCommand)

Transacciones SQL explícitas

Procedimientos almacenados (SPs) para las operaciones CRUD

Esta capa asegura integridad, performance y desac acoplamiento total con el dominio y la UI.

4. Domain (Entidades de Negocio)

Contiene las clases puras del dominio:

Cliente

Producto

Presupuesto

DetallePresupuesto

Son POCOs que representan el modelo real de negocio sin dependencias externas.

⚙️ Funcionalidad Implementada
✔ Transacciones SQL

El sistema implementa una transacción que asegura:

Se guarda el encabezado del presupuesto

Se guardan todos los detalles asociados

Si algún detalle falla, se revierte todo

Garantizando integridad y consistencia del sistema.

✔ CRUD Completo

Para cada entidad:

Crear

Leer

Actualizar

Eliminar

✔ Confirmación del Presupuesto

Funcionalidad que cambia el estado del presupuesto a Confirmado, aplicando:

Validaciones de negocio

Preparación para futura integración con el manejo de stock

🛠️ Tecnologías Utilizadas

Lenguaje: C# (.NET Framework / .NET)

Interfaz: Windows Forms (WinForms)

Base de Datos: SQL Server

Acceso a Datos: ADO.NET

Control de Versiones: Git / GitHub

Arquitectura: N-Tier / Multicapa

* Próximos Pasos

Actualmente el sistema permite gestionar clientes, productos, presupuestos y sus detalles, así como confirmar presupuestos.

Los próximos pasos planificados son:

Agregar reportes para el negocio (estadísticas, ventas, actividad).

Implementar la Lógica de Stock y Control de Inventario en la capa de Servicios. Aplicar el patrón Observer para desacoplar el evento de Presupuesto Confirmado del evento de Descuento de Stock, facilitando la escalabilidad del sistema.


Mejorar UI y experiencia de usuario.

* Estado del Proyecto

* En desarrollo
La estructura base y las funcionalidades principales ya están implementadas.
Continúa la expansión del módulo de stock y reportes.

👤 Autor

Mariano Pedrotti
Desarrollador C# / WinForms / SQL
Argentina

⚙️ 5. Instalación y Configuración de la Base de DatosPara ejecutar el proyecto, es necesario configurar la base de datos SQL Server y establecer la cadena de conexión.
Paso 1: Crear la Base de DatosAbre SQL Server Management Studio (SSMS) o tu herramienta de base de datos preferida.
Crea una nueva base de datos llamada SistemaPresupuesto.Ejecuta el script SQL completo que se encuentra en la carpeta Sql/Database.sql.txt (o el archivo que contiene la creación de todas las tablas y procedimientos almacenados).
Paso 2: Configurar la Conexión en C#La aplicación utiliza la cadena de conexión SistemaPresupuestoDB para conectarse a SQL Server.Abre el archivo App.config del proyecto.Localiza la sección <connectionStrings>.Modifica el atributo Data Source en la cadena de conexión para que apunte a tu instancia local de SQL Server.XML<connectionStrings>
    <add name="SistemaPresupuestoDB" 
         connectionString="Data Source=TU_SERVIDOR_SQL;Initial Catalog=SistemaPresupuesto;Integrated Security=True;" 
         providerName="System.Data.SqlClient" />
</connectionStrings>
Si usas SQL Express localmente, usa:Data Source=.\SQLEXPRESS
Paso 3: Ejecutar la AplicaciónAbre la solución SistemaPresupuesto.sln en Visual Studio.
Reconstruye la solución (Build > Rebuild Solution).
Ejecuta la aplicación (F5).🚀 

 Cómo Usar la Aplicación (Guion Rápido)Para probar la lógica de negocio clave:
Carga Inicial: Vaya a Gestión > Clientes y Gestión > Productos para agregar al menos un cliente y varios productos (con un valor en Stock).
Transacción Clave: Vaya a Gestión > Presupuesto.Seleccione un Cliente.Agregue múltiples productos con diferentes cantidades a la tabla de detalle.Presione Guardar. (Verifique que se guardó sin errores).
Consulta de Detalles: Vaya a Gestión > Detalle Presupuestos.Ingrese el ID del presupuesto que acaba de crear.Presione Buscar para ver la lista de detalles y verificar que el Nombre del Producto y el SubTotal se muestran correctamente (prueba de la funcionalidad de la Capa de Servicios).
