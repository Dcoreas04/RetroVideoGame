FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["RetroVideoGame.csproj", "./"]
RUN dotnet restore "./RetroVideoGame.csproj"

COPY . .
RUN dotnet build "./RetroVideoGame.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./RetroVideoGame.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

ENV DB_HOST=localhost
ENV DB_PORT=5432
ENV DB_USER=user
ENV DB_PASSWORD=password
ENV DB_NAME=GameExchangeDB

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RetroVideoGame.dll"]