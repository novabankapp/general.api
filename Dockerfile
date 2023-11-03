#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NovaPay.Integrator.General.Api/NuGet.Config", "NovaPay.Integrator.General.Api/"]
COPY ["NovaPay.Integrator.General.Api/NovaPay.Integrator.General.Api.csproj", "NovaPay.Integrator.General.Api/"]
COPY ["NovaPay.Integrator.General.Infra.IoC/NovaPay.Integrator.General.Infra.IoC.csproj", "NovaPay.Integrator.General.Infra.IoC/"]
COPY ["NovaPay.Integrator.General.Application/NovaPay.Integrator.General.Application.csproj", "NovaPay.Integrator.General.Application/"]
RUN dotnet restore "NovaPay.Integrator.General.Api/NovaPay.Integrator.General.Api.csproj" --configfile "NovaPay.Integrator.General.Api/NuGet.Config"
COPY . .
WORKDIR "/src/NovaPay.Integrator.General.Api"
RUN dotnet build "NovaPay.Integrator.General.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NovaPay.Integrator.General.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NovaPay.Integrator.General.Api.dll"]