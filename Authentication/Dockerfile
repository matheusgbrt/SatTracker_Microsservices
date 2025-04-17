# Imagem base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Imagem para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Authentication_UserManagement.csproj", "./"]
RUN dotnet restore "./Authentication_UserManagement.csproj"
COPY . .
RUN dotnet build "./Authentication_UserManagement.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Authentication_UserManagement.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authentication_UserManagement.dll"]