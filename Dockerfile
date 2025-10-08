FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY . .

RUN dotnet restore CloudGamesStore.Api/*.csproj

WORKDIR /app/CloudGamesStore.Api

RUN dotnet publish -c Release -o /dist --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /dist

COPY --from=build /dist ./

EXPOSE 80

ENTRYPOINT ["dotnet", "CloudGamesStore.Api.dll"]
