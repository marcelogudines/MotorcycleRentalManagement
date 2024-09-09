# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar os arquivos de projeto
COPY ./src/MotorcycleRentalManagement.API/MotorcycleRentalManagement.API.csproj ./src/MotorcycleRentalManagement.API/
COPY ./src/MotorcycleRentalManagement.Domain/MotorcycleRentalManagement.Domain.csproj ./src/MotorcycleRentalManagement.Domain/
COPY ./src/MotorcycleRentalManagement.Infrastructure/MotorcycleRentalManagement.Infrastructure.csproj ./src/MotorcycleRentalManagement.Infrastructure/

# Restaurar dependências para todos os projetos
RUN dotnet restore ./src/MotorcycleRentalManagement.API/MotorcycleRentalManagement.API.csproj

# Copiar o restante do código de cada projeto
COPY ./src/MotorcycleRentalManagement.API/ ./src/MotorcycleRentalManagement.API/
COPY ./src/MotorcycleRentalManagement.Domain/ ./src/MotorcycleRentalManagement.Domain/
COPY ./src/MotorcycleRentalManagement.Infrastructure/ ./src/MotorcycleRentalManagement.Infrastructure/

# Compilar e publicar a aplicação
WORKDIR /app/src/MotorcycleRentalManagement.API
RUN dotnet publish -c Release -o out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/src/MotorcycleRentalManagement.API/out .
# Expor a porta 5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "MotorcycleRentalManagement.API.dll"]
