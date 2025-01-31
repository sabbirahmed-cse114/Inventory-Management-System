FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
RUN apt update && apt install -y nodejs
COPY ["REC.Inventory.Web/*.csproj", "REC.Inventory.Web/"]
COPY ["REC.Inventory.Application/*.csproj", "REC.Inventory.Application/"]
COPY ["REC.Inventory.Domain/*.csproj", "REC.Inventory.Domain/"]
COPY ["REC.Inventory.Infrastructure/*.csproj", "REC.Inventory.Infrastructure/"]
RUN dotnet restore "REC.Inventory.Web/REC.Inventory.Web.csproj"
COPY . .
WORKDIR "/src/REC.Inventory.Web"
RUN dotnet build "REC.Inventory.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "REC.Inventory.Web.csproj" -c Release -o /app

FROM build AS final
WORKDIR /app
COPY --from=publish /app .
EXPOSE 80
ENTRYPOINT ["dotnet", "REC.Inventory.Web.dll"]