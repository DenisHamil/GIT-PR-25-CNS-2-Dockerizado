# Usar imagen base de .NET para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Usar imagen base de .NET para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solo el archivo .csproj y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto del código y publicar
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Combinar
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PRIII-24_CONTROL_ANTIBIOTICOS.dll"]
