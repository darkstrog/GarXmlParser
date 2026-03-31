FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /build

# Копируем SLN файл вместе с его родительской структурой
COPY solutions/GARReplication /build/solutions/GARReplication
COPY src /build/src

# Устанавливаем рабочую директорию туда, где находится SLN файл
WORKDIR /build/solutions/GARReplication

# Восстанавливаем зависимости
RUN dotnet restore "GARReplication.sln"

# Публикуем проект (путь относительно SLN файла)
WORKDIR /build/src/services/GARReplication/GARReplication
RUN dotnet publish "GARReplication.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GARReplication.dll"]
