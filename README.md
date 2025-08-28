# 🏢 ECS Clicker

Idle clicker игра на чистой **Entity-Component-System** архитектуре с **LeoEcsLite**.

![Unity](https://img.shields.io/badge/Unity-2022.3-blue.svg) ![ECS](https://img.shields.io/badge/ECS-LeoEcsLite-green.svg)

## 🎮 Геймплей

- **5 бизнесов** с прогрессивным ростом стоимости
- **PowerUp система** для увеличения дохода  
- **Автосохранение** при сворачивании приложения
- **Визуальные таймеры** генерации дохода

## 🏗️ Архитектура

```
ECS Systems:
├── IncomeSystem          # Генерация дохода
├── BuyLevelUpSystem      # Покупка уровней
├── BuyPowerUpSystem      # Покупка улучшений
├── UpdateSliderSystem    # Прогресс-бары
└── SaveGameSystem        # Автосохранение
```

**Формулы:**
- Доход: `уровень × базовый_доход × (1 + PowerUp_мультипликаторы)`
- Цена: `(уровень + 1) × базовая_стоимость`

## 📁 Структура

```
Assets/
├── Scripts/
│   ├── Components/       # ECS компоненты
│   ├── Systems/         # ECS системы
│   ├── Services/        # Фабрика, сохранения
│   └── StaticData/      # ScriptableObject конфиги
├── Resources/
│   ├── Static Data/     # Настройки бизнесов/PowerUp
│   └── Prefabs/        # UI префабы
└── Plugins/ecslite/    # LeoEcsLite фреймворк
```

## 🚀 Запуск

1. Открыть `Assets/Scenes/Main.unity`
2. Нажать **Play**
3. `EcsStartup` инициализирует все системы

## ⚙️ Конфигурация

Настройки в `Assets/Resources/Static Data/`:
- **Businesses/**: 5 бизнесов (доходность, стоимость, таймеры)
- **PowerUps/**: Улучшения (по 2 на бизнес)

## 🛠️ Технологии

- **Unity 2022.3**
- **LeoEcsLite 2023.2.22**
- **Чистая ECS** без DI фреймворков
- **ScriptableObject** конфигурации
- **PlayerPrefs** для сохранений