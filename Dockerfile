# Multi-stage .NET 8 build.
# Build context: repository root.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Domain/EventFlow.Domain.csproj", "src/Domain/"]
COPY ["src/Events/EventFlow.Events.csproj", "src/Events/"]
COPY ["src/Application/EventFlow.Application.csproj", "src/Application/"]
COPY ["src/Infrastructure/EventFlow.Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Messaging/EventFlow.Messaging.csproj", "src/Messaging/"]
COPY ["src/WebAPI/EventFlow.WebAPI.csproj", "src/WebAPI/"]

RUN dotnet restore "src/WebAPI/EventFlow.WebAPI.csproj"

COPY . .
RUN dotnet publish "src/WebAPI/EventFlow.WebAPI.csproj" \
    --configuration Release \
    --no-restore \
    --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_HTTP_PORTS=8080
ENV DOTNET_EnableDiagnostics=0

COPY --from=build /app/publish .
EXPOSE 8080

ENTRYPOINT ["dotnet", "EventFlow.WebAPI.dll"]
