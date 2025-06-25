# 🧪 PRIII-24_CONTROL_ANTIBIOTICOS – Sistema de Gestión de Antibióticos

Este proyecto contiene una aplicación ASP.NET Core 8.0 **dockerizada**, junto con su base de datos **SQL Server**, diseñada para el **control y gestión de antibióticos** en entornos clínicos.  
Todo está listo para ser ejecutado en cualquier servidor o entorno local mediante Docker.

---

## 📂 Contenido del Repositorio

| Archivo                 | Descripción                                     |
|------------------------|-------------------------------------------------|
| `priii-webapp.tar`     | Imagen Docker de la aplicación ASP.NET Core    |
| `sqlserver.tar`        | Imagen Docker oficial de SQL Server 2022       |
| `.gitattributes`       | Configuración para Git LFS                     |

---

## 🛠️ Requisitos

Antes de ejecutar el proyecto, asegúrate de tener instalado:

- ✅ [Git](https://git-scm.com/downloads)
- ✅ [Git LFS](https://git-lfs.com/)
- ✅ [Docker](https://www.docker.com/products/docker-desktop)

---

## 🚀 Pasos para ejecutar el sistema

### 1. Clonar el repositorio (con soporte para archivos grandes)

```bash
git lfs install
git clone https://github.com/DenisHamil/GIT-PR-25-CNS-2-Dockerizado.git
cd GIT-PR-25-CNS-2-Dockerizado/PRIII-24_CONTROL_ANTIBIOTICOS
```

---

### 2. Cargar las imágenes Docker desde los `.tar`

```bash
docker load -i priii-webapp.tar
docker load -i sqlserver.tar
```

---

### 3. Levantar los contenedores

#### 3.1 Iniciar SQL Server

```bash
docker run -d ^
  --name sqlserver ^
  -e "ACCEPT_EULA=Y" ^
  -e "SA_PASSWORD=Denis71463825" ^
  -p 1433:1433 ^
  mcr.microsoft.com/mssql/server:2022-latest
```

#### 3.2 Iniciar la aplicación ASP.NET Core

```bash
docker run -d ^
  --name bdproa_app ^
  -p 5000:8080 ^
  priii-24_control_antibioticos-webapp
```

---

## 🌐 Acceso a la aplicación

- En entorno local:  
  👉 [http://localhost:5000](http://localhost:5000)

- En VPS:  
  👉 `http://IP_DEL_SERVIDOR:5000`

---

## 🔐 Datos de acceso

| Usuario  | Contraseña |
|----------|------------|
| admin    | admin      |

Este es el único rol habilitado actualmente.

---

## 📝 Notas adicionales

- Si necesitas reconstruir las imágenes desde los contenedores locales:

```bash
docker commit bdproa_app priii-24_control_antibioticos-webapp
docker save -o priii-webapp.tar priii-24_control_antibioticos-webapp
docker save -o sqlserver.tar mcr.microsoft.com/mssql/server:2022-latest
```

---

## 📄 Licencia

Este proyecto es de uso académico y puede adaptarse libremente con fines educativos.
