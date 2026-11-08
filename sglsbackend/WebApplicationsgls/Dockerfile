FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["WebApplicationsgls.API/WebApplicationsgls.API.csproj", "WebApplicationsgls.API/"]
COPY ["WebApplicationsgls.Application/WebApplicationsgls.Application.csproj", "WebApplicationsgls.Application/"]
COPY ["WebApplicationsgls.Domain/WebApplicationsgls.Domain.csproj", "WebApplicationsgls.Domain/"]
COPY ["WebApplicationsgls.Infrastructure/WebApplicationsgls.Infrastructure.csproj", "WebApplicationsgls.Infrastructure/"]

RUN dotnet restore "WebApplicationsgls.API/WebApplicationsgls.API.csproj"

COPY . .

WORKDIR /src/WebApplicationsgls.API

RUN dotnet publish "WebApplicationsgls.API.csproj" \
    --configuration Release \
    --output /app/publish \
    /p:UseAppHost=false



FROM build AS migrations

RUN dotnet tool install --global dotnet-ef --version "8.*"

ENV PATH="/root/.dotnet/tools:${PATH}"

WORKDIR /src/WebApplicationsgls.API

ENTRYPOINT ["dotnet", "ef", "database", "update", "--project", "../WebApplicationsgls.Infrastructure/WebApplicationsgls.Infrastructure.csproj", "--startup-project", "WebApplicationsgls.API.csproj"]

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "WebApplicationsgls.API.dll"]