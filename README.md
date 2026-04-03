# TechLogistics Enterprise

![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Angular](https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white)

# TechLogistics Enterprise: Sistema Distribuido de Gestión Logística

TechLogistics es una solución Fullstack de nivel empresarial diseñada bajo el paradigma de Arquitectura Orientada a Eventos (EDA) y Clean Architecture. El objetivo principal es gestionar envíos de forma asíncrona, garantizando escalabilidad y resiliencia mediante el procesamiento en segundo plano.

## 🏗️ Arquitectura del Sistema

El proyecto implementa una separación de responsabilidades estricta para asegurar la mantenibilidad a largo plazo:
* API (Producer): Punto de entrada RESTful que valida solicitudes, persiste el estado inicial en SQL Server y publica eventos de despacho.
* Worker (Consumer): Servicio de fondo (BackgroundService) que consume mensajes de forma resiliente, simulando tareas intensivas como generación de documentos y notificaciones.
* Broker (RabbitMQ): Actúa como buffer de mensajes asegurando que ninguna solicitud se pierda incluso ante caídas del consumidor.
* Domain & Application: Capas agnósticas a la infraestructura que contienen la verdad del negocio y las interfaces de comunicación.

## 🛠️ Stack Tecnológico

### Backend & DevOps
* Core: .NET 9 con C# 12.
* Persistencia: Entity Framework Core con SQL Server.
* Mensajería: RabbitMQ (CloudAMQP) para comunicación asíncrona.
* Testing: xUnit para unit testing y Moq para aislamiento de dependencias.
### Frontend
* Framework: Angular 17+ con Standalone Components para un bundle ligero.

## 🧠 Decisiones de Diseño
* Idempotencia en Mensajería: El Worker declara la cola de forma automática al iniciar (QueueDeclare), permitiendo que el sistema sea resiliente a fallos en el orden de encendido de los servicios.
* Validación de Dominio: La entidad Shipping garantiza que no existan envíos sin destinatario mediante validaciones en su constructor, protegiendo la integridad de los datos desde la raíz.
* CORS Policy: Se implementó una política restrictiva para permitir únicamente el origen del Frontend, siguiendo las mejores prácticas de seguridad en APIs web.
* Confirmación de Entrega (ACK): El Worker solo confirma el mensaje (BasicAck) tras procesar exitosamente la tarea, evitando la pérdida de mensajes en caso de errores inesperados durante la ejecución.

## 🚀 Guía de Configuración

### Prerrequisitos
* .NET 9 SDK
* Node.js (LTS) & Angular CLI
* SQL Server (Local o Docker)
* Instancia de RabbitMQ (CloudAMQP recomendado)

### 1. Configuración del Backend
1.  Navega a la carpeta raíz.
2.  Configura las cadenas de conexión en `src/TechLogistics.API/appsettings.json` y `src/TechLogistics.Worker/appsettings.json`:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=.;Database=TechLogisticsDB;Trusted_Connection=True;TrustServerCertificate=True;",
      "RabbitMQConnection": "TU_URL_DE_CLOUDAMQP"
    }
    ```
3.  Ejecutar Migraciones:
    ```bash
    dotnet ef database update --project src/TechLogistics.Infrastructure
    ```

4.  Iniciar Servicios:
    Es necesario ejecutar tanto la API como el Worker simultáneamente:
    ```bash
    # En terminal 1
    dotnet run --project src/TechLogistics.API

    # En terminal 2
    dotnet run --project src/TechLogistics.Worker
    ```

## 🧪 Estrategia de Testing
El proyecto cuenta con una suite de pruebas robusta:
* Pruebas de Dominio: Validan la generación de Tracking IDs y lógica de estados.
* Pruebas de Controlador: Verifican que la API coordine correctamente el repositorio y el sistema de mensajes ante una solicitud.
Ejecutar todas las pruebas con:
```bash
    dotnet test
```
