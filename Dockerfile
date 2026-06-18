FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["MangaT.API/MangaT.API.csproj", "MangaT.API/"]
COPY ["MangaT.ApplicationCore/MangaT.ApplicationCore.csproj", "MangaT.ApplicationCore/"]
COPY ["MangaT.Domain/MangaT.Domain.csproj", "MangaT.Domain/"]
COPY ["MangaT.Infrastructure/MangaT.Infrastructure.csproj", "MangaT.Infrastructure/"]

RUN dotnet restore "MangaT.API/MangaT.API.csproj"

COPY . .
WORKDIR /src/MangaT.API
RUN dotnet build "MangaT.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MangaT.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MangaT.API.dll"]
