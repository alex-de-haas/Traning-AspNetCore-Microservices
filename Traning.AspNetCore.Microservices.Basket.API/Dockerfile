#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Traning.AspNetCore.Microservices.Basket.API/Traning.AspNetCore.Microservices.Basket.API.csproj", "Traning.AspNetCore.Microservices.Basket.API/"]
RUN dotnet restore "Traning.AspNetCore.Microservices.Basket.API/Traning.AspNetCore.Microservices.Basket.API.csproj"
COPY . .
WORKDIR "/src/Traning.AspNetCore.Microservices.Basket.API"
RUN dotnet build "Traning.AspNetCore.Microservices.Basket.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Traning.AspNetCore.Microservices.Basket.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Traning.AspNetCore.Microservices.Basket.API.dll"]