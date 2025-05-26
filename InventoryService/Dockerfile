# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "InventoryService.csproj"
RUN dotnet build "InventoryService.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "InventoryService.csproj" -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8081
ENTRYPOINT ["dotnet", "InventoryService.dll"]

