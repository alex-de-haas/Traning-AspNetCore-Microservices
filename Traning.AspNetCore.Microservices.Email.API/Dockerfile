#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Traning.AspNetCore.Microservices.Email.API/Traning.AspNetCore.Microservices.Email.API.csproj", "Traning.AspNetCore.Microservices.Email.API/"]
RUN dotnet restore "Traning.AspNetCore.Microservices.Email.API/Traning.AspNetCore.Microservices.Email.API.csproj"
COPY . .
WORKDIR "/src/Traning.AspNetCore.Microservices.Email.API"
RUN dotnet build "Traning.AspNetCore.Microservices.Email.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Traning.AspNetCore.Microservices.Email.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Traning.AspNetCore.Microservices.Email.API.dll"]