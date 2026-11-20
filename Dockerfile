# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files for caching restore layer
COPY ["AppLS10.sln", "./"]
COPY ["AppLS10.api/AppLS10.api.csproj", "AppLS10.api/"]
COPY ["AppLS10.application/AppLS10.application.csproj", "AppLS10.application/"]
COPY ["AppLS10.domain/AppLS10.domain.csproj", "AppLS10.domain/"]
COPY ["AppLS10.infrastructure/AppLS10.infrastructure.csproj", "AppLS10.infrastructure/"]

RUN dotnet restore "AppLS10.sln"

# Copy full source and publish
COPY . .
WORKDIR "/src/AppLS10.api"
RUN dotnet publish "AppLS10.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "AppLS10.api.dll"]
