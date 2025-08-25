# RSS Aplikace s React Navigation

Tato aplikace demonstruje implementaci bottom tab navigace v React Native pomocí Expo.

## Instalace

Nejdříve nainstalujte potřebné navigační balíčky:

```bash
npm install @react-navigation/native @react-navigation/bottom-tabs react-native-screens react-native-safe-area-context
```

## Struktura složek

```
src/
├── components/          # Znovupoužitelné komponenty
├── screens/            # Obrazovky aplikace
│   ├── HomeScreen.js   # Domovská obrazovka
│   ├── FeedScreen.js   # RSS feedy
│   └── SettingsScreen.js # Nastavení
├── navigation/         # Navigační konfigurace
│   └── TabNavigator.js # Bottom tab navigace
├── constants/          # Konstanty (barvy, velikosti)
│   └── Colors.js      # Definice barev
└── utils/              # Pomocné funkce
```

## Funkce

- **Domů**: Uvítací obrazovka
- **RSS Feedy**: Seznam RSS feedů s mock daty
- **Nastavení**: Nastavení aplikace s přepínači a tlačítky

## Spuštění

```bash
npm start
```

## Navigace

Aplikace používá `@react-navigation/bottom-tabs` pro vytvoření spodní navigační lišty s třemi záložkami. Každá záložka má vlastní ikonu a barvu.

## Customizace

Barvy a styly lze upravit v `src/constants/Colors.js`. Navigační možnosti lze upravit v `src/navigation/TabNavigator.js`.
