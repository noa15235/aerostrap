<div align="center">

<img src="Images/logo.png" width="96" alt="Aerostrap" />

# Aerostrap

Launcher Roblox léger pour Windows : FastFlags, mods, Rich Presence Discord et réglages de performance.

[![Build](https://github.com/noa15235/aerostrap/actions/workflows/build.yml/badge.svg)](https://github.com/noa15235/aerostrap/actions/workflows/build.yml)
[![Release](https://img.shields.io/github/v/release/noa15235/aerostrap)](https://github.com/noa15235/aerostrap/releases/latest)

</div>

## Fonctionnalités

- **Preset performance** en un clic : MSAA désactivé et Roblox lancé en priorité haute
- **Rich Presence Discord** personnalisable : jeu, créateur, minuteur, avatar et textes avec variables (`{game}`, `{creator}`, `{placeId}`, `{jobId}`)
- **FastFlags** : réglages de rendu et éditeur complet
- **Mods** : curseurs, sons, polices et emojis
- **Choix du canal** Roblox et mises à jour automatiques
- **Indication de la région du serveur** et historique des parties
- Interface claire / sombre, traduite en plus de 30 langues
- Aucune télémétrie

## Installation

1. Télécharge `Aerostrap.exe` depuis les [Releases](https://github.com/noa15235/aerostrap/releases/latest).
2. Lance-le et suis l'installateur.
3. Ouvre Roblox depuis le site ou le raccourci : Aerostrap prend le relais.

Windows 10 ou 11 et [.NET 6 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/6.0) requis.

## Build

```bash
dotnet publish Aerostrap/Aerostrap.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true
```

## Licence

[MIT](LICENSE)
