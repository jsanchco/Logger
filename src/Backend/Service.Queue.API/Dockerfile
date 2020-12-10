#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Backend/Service.Queue.API/Service.Queue.API.csproj", "src/Backend/Service.Queue.API/"]
COPY ["src/Common/Common.Domain/Common.Domain.csproj", "src/Common/Common.Domain/"]
COPY ["src/Common/Common.Logging/Common.Logging.csproj", "src/Common/Common.Logging/"]
COPY ["src/EventBus/EventBus.Common/EventBus.Common.csproj", "src/EventBus/EventBus.Common/"]
COPY ["src/EventBus/EventBus.Rabbit/EventBus.Rabbit.csproj", "src/EventBus/EventBus.Rabbit/"]
COPY ["src/Backend/ServiceList/ServiceList.csproj", "src/Backend/ServiceList/"]
COPY ["src/Backend/Persistence/Persistence.MongoDB/Persistence.MongoDB.csproj", "src/Backend/Persistence/Persistence.MongoDB/"]
COPY ["src/Backend/Persistence/Persistence.Repository/Persistence.Repository.csproj", "src/Backend/Persistence/Persistence.Repository/"]
COPY ["src/Common/Common.Pagination/Common.Pagination.csproj", "src/Common/Common.Pagination/"]
COPY ["src/Backend/Persistence/Persistence.Service/Persistence.Service.csproj", "src/Backend/Persistence/Persistence.Service/"]
RUN dotnet restore "src/Backend/Service.Queue.API/Service.Queue.API.csproj"
COPY . .
WORKDIR "/src/src/Backend/Service.Queue.API"
RUN dotnet build "Service.Queue.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Service.Queue.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Service.Queue.API.dll"]