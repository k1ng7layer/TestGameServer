FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TestGameServer.csproj", "./"]
RUN dotnet restore "TestGameServer.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "TestGameServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestGameServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestGameServer.dll"]
