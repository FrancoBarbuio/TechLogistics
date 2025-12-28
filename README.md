# TechLogistics Enterprise

Sistema de gestión logística distribuido implementando una **Arquitectura Orientada a Eventos (EDA)**.
Este proyecto demuestra la integración Fullstack entre un Frontend moderno, una API RESTful, persistencia de datos y procesamiento asíncrono en segundo plano mediante colas de mensajes.

## Arquitectura del Sistema

El sistema sigue el principio de **Desacoplamiento de Responsabilidades**:
1.  **Cliente (Frontend):** Envía la solicitud de envío.
2.  **API (Producer):** Valida, guarda el estado inicial en SQL y delega el procesamiento pesado a RabbitMQ.
3.  **Broker (RabbitMQ):** Garantiza la entrega del mensaje (Durable Queue).
4.  **Worker (Consumer):** Procesa la tarea en segundo plano (simulación de generación de PDF y notificación) sin bloquear al usuario.

## Tecnologías (Tech Stack)

### Backend (.NET 9)
* **TechLogistics.API:** Web API RESTful.
* **TechLogistics.Worker:** Servicio en segundo plano (BackgroundService).
* **TechLogistics.Domain:** Lógica de negocio pura (DDD).
* **TechLogistics.Infrastructure:** Implementación de persistencia y mensajería.
* **Entity Framework Core:** ORM para SQL Server.
* **RabbitMQ.Client:** Comunicación con el Message Broker.
* **xUnit & Moq:** Pruebas unitarias y Mocking.

### Frontend
* **Angular 17+:** Standalone Components.
* **TypeScript:** Tipado fuerte.

### Infraestructura
* **SQL Server:** Base de datos relacional.
* **CloudAMQP (RabbitMQ):** Instancia de RabbitMQ en la nube.

## Guía de Inicio (Setup)

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
3.  Ejecuta la API:
    ```bash
    dotnet run --project src/TechLogistics.API/TechLogistics.API.csproj
    ```

### 2. Ejecución del Worker
En una nueva terminal:
```bash
dotnet run --project src/TechLogistics.Worker/TechLogistics.Worker.csproj
