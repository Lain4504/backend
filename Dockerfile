# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 3000
EXPOSE 3001

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Development
WORKDIR /src

# Sao chép tệp .csproj
COPY backend/BackEnd.csproj ./backend/

# Chạy restore trong thư mục đúng
RUN dotnet restore "./backend/BackEnd.csproj"

# Sao chép tất cả mã nguồn backend
COPY backend/. ./backend/  

WORKDIR "/src/backend/"
RUN dotnet build "BackEnd.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Development
RUN dotnet publish "BackEnd.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish . 
ENTRYPOINT ["dotnet", "BackEnd.dll"]
