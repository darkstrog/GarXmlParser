FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /build

COPY ["GarXmlParser.sln", "."]
COPY ["src/GarXmlParser/GarXmlParser.csproj", "src/GarXmlParser/"]
COPY ["src/GarXmlParserConsole/GarXmlParserConsole.csproj", "src/GarXmlParserConsole/"]
COPY ["src/GAROperations.Core/GAROperations.Core.csproj", "src/GAROperations.Core/"]
COPY ["src/GAROperations.Infrastructure/GAROperations.Infrastructure.csproj", "src/GAROperations.Infrastructure/"]
COPY ["src/GAROperations.WebApi/GAROperations.WebApi.csproj", "src/GAROperations.WebApi/"]

RUN dotnet restore "GarXmlParser.sln"

COPY . .
WORKDIR "/build/src/GarXmlParserConsole"
RUN dotnet publish "GarXmlParserConsole.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GarXmlParserConsole.dll"]