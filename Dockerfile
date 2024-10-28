# Use the ASP.NET base image with Alpine
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 3000
EXPOSE 3001

# Use the .NET SDK base image with Alpine for building the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY backend/BackEnd.csproj ./backend/
RUN dotnet restore "./backend/BackEnd.csproj"

# Copy all backend source code
COPY backend/. ./backend/  

WORKDIR "/src/backend/"
RUN dotnet build "BackEnd.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "BackEnd.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage for production
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish . 
ENTRYPOINT ["dotnet", "BackEnd.dll"]
