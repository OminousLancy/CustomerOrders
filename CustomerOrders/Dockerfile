FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CustomerOrders/CustomerOrders.csproj", "CustomerOrders/"]
COPY ["CustomerOrders.Application/CustomerOrders.WebApi.csproj", "CustomerOrders.WebApi/"]
COPY ["CustomerOrders.Application/CustomerOrders.Application.csproj", "CustomerOrders.Application/"]
COPY ["CustomerOrders.Domain/CustomerOrders.Domain.csproj", "CustomerOrders.Domain/"]
COPY ["CustomerOrders.Infrastructure/CustomerOrders.Infrastructure.csproj", "CustomerOrders.Infrastructure/"]

RUN dotnet restore "CustomerOrders/CustomerOrders.csproj"
COPY . .
WORKDIR "/src/CustomerOrders"
RUN dotnet build "CustomerOrders.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CustomerOrders.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerOrders.dll"]
