FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["MyCefet.Api/MyCefet.Api.csproj", "MyCefet.Api/"]
RUN dotnet restore "MyCefet.Api/MyCefet.Api.csproj"
COPY . .
WORKDIR "/src/MyCefet.Api"
RUN dotnet build "MyCefet.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MyCefet.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MyCefet.Api.dll"]