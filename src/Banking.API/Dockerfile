FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Banking.API/Banking.API.csproj", "Banking.API/"]
COPY ["Banking.Application/Banking.Application.csproj", "Banking.Application/"]
COPY ["Banking.Domain/Banking.Domain.csproj", "Banking.Domain/"]
COPY ["Banking.Infrastructure/Banking.API.csproj", "Banking.Infrastructure/"]
RUN dotnet restore "./Banking.API/Banking.API.csproj"
COPY . .
WORKDIR "/src/Banking.API"
RUN dotnet build "./Banking.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Banking.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Banking.API.dll"]