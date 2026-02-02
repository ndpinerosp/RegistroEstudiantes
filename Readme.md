# Proyecto Full Stack - .NET Core 8 + Angular 19

Este repositorio contiene una aplicación **Full Stack** compuesta por un **Backend** desarrollado en **.NET Core 8 (ASP.NET Core Web API)** y un **Frontend** desarrollado en **Angular 19**.


---

## Tecnologías Utilizadas

### Backend
- .NET Core 8
- ASP.NET Core Web API
- HTTPS
- Arquitectura basada en controladores

### Frontend
- Angular 19
- TypeScript
- Angular CLI
- HttpClient para consumo de la API

---
## Base de datos

Mysql  8.0

## Estructura General del Proyecto

El proyecto se divide en dos carpetas principales:

```text
/
├── RegistroEstudiantesApi/        -> Proyecto ASP.NET Core 8 (API)
└── RegistroEstudiantes.Client/       -> Proyecto Angular 19
```

### Configuración y Ejecución

1. Primero, ejecuta el Backend (API) de .NET Core. con Visual Studio 

2. Copia la URL generada por la API (por ejemplo: https://localhost:5001).

3. Abre el proyecto Angular (RegistroEstudiantes.Client) y busca el archivo **proxy.conf.json**.

4. Configura la dirección de la API para evitar problemas de CORS, de la siguiente manera:
```code
{
  "/api": {
    "target": "https://localhost:{PuertoApi}",
    "secure": false,
    "changeOrigin": true,
    "logLevel": "debug"
  }
}

```
5. Ejecuta angular con el comando ng serve

Adjunto se encentran los scripts SQl para creacion y cargue de datos.