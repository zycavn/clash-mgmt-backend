FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build

ARG BUILD_TIME
ENV ASPNETCORE_ENVIRONMENT=$BUILD_TIME

WORKDIR /app

# copy csproj and restore as distinct layers
COPY ClashServer/ClashServer.csproj ./ClashServer/
RUN dotnet restore ClashServer

# copy LoggerService csproj and restore as distinct layers
COPY LoggerService/LoggerService.csproj ./LoggerService/
RUN dotnet restore LoggerService

# copy everything else and build app
COPY ClashServer/. ./ClashServer/
COPY LoggerService/. ./LoggerService/

WORKDIR /app/ClashServer
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS runtime
WORKDIR /app
COPY --from=build /app/ClashServer/out ./
ENTRYPOINT ["dotnet", "ClashServer.dll"]
RUN mkdir -p ./Resources
