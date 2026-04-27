FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia apenas os .csproj para aproveitar o cache de camadas do Docker
COPY ["IgrejaV2.API/IgrejaV2.API.csproj", "IgrejaV2.API/"]
COPY ["IgrejaV2.Aplicacao/IgrejaV2.Aplicacao.csproj", "IgrejaV2.Aplicacao/"]
COPY ["IgrejaV2.Infraestrutura/IgrejaV2.Infraestrutura.csproj", "IgrejaV2.Infraestrutura/"]
COPY ["IgrejaV2.Dominio/IgrejaV2.Dominio.csproj", "IgrejaV2.Dominio/"]
RUN dotnet restore "IgrejaV2.API/IgrejaV2.API.csproj"

# Copia o restante do código e compila
COPY . .
WORKDIR "/src/IgrejaV2.API"
RUN dotnet build "IgrejaV2.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IgrejaV2.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IgrejaV2.API.dll"]
