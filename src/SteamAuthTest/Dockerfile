FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/SteamAuthTest/SteamAuthTest.csproj", "src/SteamAuthTest/"]
RUN dotnet restore "src/SteamAuthTest/SteamAuthTest.csproj"
COPY . .
WORKDIR "/src/src/SteamAuthTest"
RUN dotnet build "SteamAuthTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SteamAuthTest.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SteamAuthTest.dll"]
