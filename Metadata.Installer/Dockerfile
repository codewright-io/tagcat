#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Metadata.Installer/Metadata.Installer.csproj", "Metadata.Installer/"]
COPY ["Metadata.API/Metadata.API.csproj", "Metadata.API/"]
COPY ["Common/Common/Common.csproj", "Common/Common/"]
COPY ["Common/Common.Asp/Common.Asp.csproj", "Common/Common.Asp/"]
COPY ["Common/Common.EventSourcing/Common.EventSourcing.csproj", "Common/Common.EventSourcing/"]
COPY ["Common/Common.EventSourcing.EntityFramework/Common.EventSourcing.EntityFramework.csproj", "Common/Common.EventSourcing.EntityFramework/"]
RUN dotnet restore "Metadata.API/Metadata.API.csproj"
COPY . .
WORKDIR "/src/Metadata.Installer"
RUN dotnet build "Metadata.Installer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Metadata.Installer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeWright.Metadata.Installer.dll"]