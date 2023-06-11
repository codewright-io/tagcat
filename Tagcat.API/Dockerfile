#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5551

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Tagcat.API/Tagcat.API.csproj", "Tagcat.API/"]
COPY ["Submodules/Common/Common/Common.csproj", "Common/Common/"]
COPY ["Submodules/Common/Common.Asp/Common.Asp.csproj", "Common/Common.Asp/"]
COPY ["Submodules/Common/Common.EventSourcing/Common.EventSourcing.csproj", "Common/Common.EventSourcing/"]
COPY ["Submodules/Common/Common.EventSourcing.EntityFramework/Common.EventSourcing.EntityFramework.csproj", "Common/Common.EventSourcing.EntityFramework/"]
RUN dotnet restore "Tagcat.API/Tagcat.API.csproj"
COPY . .
WORKDIR "/src/Tagcat.API"
RUN dotnet build "Tagcat.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tagcat.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5551
ENTRYPOINT ["dotnet", "CodeWright.Tagcat.API.dll"]