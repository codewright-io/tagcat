#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5551

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
RUN git submodule update --init --recursive
COPY ["Metadata.API/Metadata.API.csproj", "Metadata.API/"]
COPY ["Submodules/Common/Common/Common.csproj", "Common/Common/"]
COPY ["Submodules/Common/Common.Asp/Common.Asp.csproj", "Common/Common.Asp/"]
COPY ["Submodules/Common/Common.EventSourcing/Common.EventSourcing.csproj", "Common/Common.EventSourcing/"]
COPY ["Submodules/Common/Common.EventSourcing.EntityFramework/Common.EventSourcing.EntityFramework.csproj", "Common/Common.EventSourcing.EntityFramework/"]
RUN dotnet restore "Metadata.API/Metadata.API.csproj"
COPY . .
WORKDIR "/src/Metadata.API"
RUN dotnet build "Metadata.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Metadata.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5551
ENTRYPOINT ["dotnet", "CodeWright.Metadata.API.dll"]