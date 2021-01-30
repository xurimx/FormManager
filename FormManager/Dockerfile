#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src
COPY ["FormManager/FormManager.Api.csproj", "FormManager/"]
COPY ["FormManager.Infrastructure/FormManager.Infrastructure.csproj", "FormManager.Infrastructure/"]
COPY ["FormManager.Application/FormManager.Application.csproj", "FormManager.Application/"]
COPY ["FormManager.Domain/FormManager.Domain.csproj", "FormManager.Domain/"]
RUN dotnet restore "FormManager/FormManager.Api.csproj"
COPY . .
WORKDIR "/src/FormManager"
RUN dotnet build "FormManager.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FormManager.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FormManager.Api.dll"]
