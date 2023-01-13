#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

COPY . /src
WORKDIR /src/OrderApi
RUN dotnet restore "OrderApi.csproj"
RUN dotnet build "OrderApi.csproj" --no-restore -c Release

FROM build AS publish
RUN dotnet publish "OrderApi.csproj" --no-build --no-restore -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "OrderApi.dll"]