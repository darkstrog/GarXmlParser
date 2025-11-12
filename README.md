# GarXmlParser

Быстрый запуск
Предварительные требования

    Docker Desktop

    Docker Compose

Запуск демо

Клонируйте репозиторий

	git clone <repository-url>
	cd gar-parser

Настройте переменные окружения

# Скопируйте и отредактируйте .env файл

	cp .env.example .env

Отредактируйте .env:

	DB_PASSWORD=your_password
	ConnectionStrings_DefaultConnection="Host=localhost;Port=5433;Database=GARBase;Username=postgres;Password=your_password"
	SourceSettings_ZipSourcePath=/data/your_data_file.zip
	RegionsSettings_Default=["50","77","90","99"]

	RegionsSettings_Default="" - для обработки всех регионов

Запустите приложение

	docker-compose up -d

Доступ к интерфейсам:

    Приложение: работает в контейнере GARparser

    База данных: PostgreSQL на localhost:5432

    Админка БД: pgAdmin на http://localhost:8080

Доступ к pgAdmin:

    Откройте http://localhost:8080

    Войдите с учеткой:

        Email: admin@gar.demo

        Пароль: admin123

    Сервер GAR Database будет предварительно настроен

Структура проекта:

	garxmlparser/
	├── docker-compose.yml    # Конфигурация всех сервисов
	├── .env                  # Переменные окружения (создать из .env.example)
	├── GarXmlParser.sln	  # Файл решения
	├── src/                  # Исходный код приложения
	├── db/
	│   └── init/             # SQL скрипты инициализации БД
	├── data/				  # Исходные данные в xml формате упакованные в zip архив
	└── pgadmin/
	     └── servers.json     # Конфигурация pgAdmin


Сервисы

    parser-console: Демо приложение парсера

    db: PostgreSQL база данных

    pgadmin: Веб-интерфейс для управления БД

Управление контейнерами


	# Запуск
	docker-compose up -d

	# Остановка
	docker-compose down

	# Просмотр логов
	docker-compose logs parser-console
	docker-compose logs db

	# Полная пересборка
	docker-compose down -v
	docker-compose up --build


Важные заметки:

    Приложение автоматически ждет полного запуска БД перед началом работы

    Все пути к файлам данных указываются в .env

    Данные БД сохраняются в Docker volume postgres_data

    pgAdmin может запускаться некоторое время поэтому будет доступен не сразу

Решение проблем:

Приложение не подключается к БД:

    Убедитесь, что БД полностью запустилась: docker-compose logs db

    Проверьте пароль в .env файле

Нужно изменить конфигурацию:

    Отредактируйте .env файл

    Перезапустите: docker-compose down && docker-compose up -d
