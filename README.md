<p align="center">
  <img src="Images/logo.png" width="104" alt="Aerostrap">
</p>

<h1 align="center">Aerostrap</h1>

<p align="center">
  Un launcher Roblox pour Windows, léger et facile à personnaliser.
</p>

<p align="center">
  <a href="https://github.com/noa15235/aerostrap/releases/latest"><img src="https://img.shields.io/github/v/release/noa15235/aerostrap?style=flat-square&color=4F7CFF&label=version" alt="Version"></a>
  <a href="https://github.com/noa15235/aerostrap/releases"><img src="https://img.shields.io/github/downloads/noa15235/aerostrap/total?style=flat-square&color=4F7CFF&label=t%C3%A9l%C3%A9chargements" alt="Téléchargements"></a>
  <a href="https://github.com/noa15235/aerostrap/actions/workflows/build.yml"><img src="https://img.shields.io/github/actions/workflow/status/noa15235/aerostrap/build.yml?style=flat-square&label=build" alt="Build"></a>
</p>

<p align="center">
  <img src="Images/launcher.png" width="49%" alt="Menu d'accueil">
  <img src="Images/bootstrapper.png" width="42%" alt="Fenêtre de lancement">
</p>

Aerostrap prend la place du lanceur officiel. Quand tu cliques sur « Jouer » sur le site de Roblox, c'est lui qui s'ouvre : il installe ou met à jour Roblox, applique tes réglages, puis lance le jeu.

## Ce qu'il fait

- **Rich Presence Discord** : le jeu, son icône, le temps passé dessus et un bouton pour rejoindre ton serveur. Tu peux réécrire les deux lignes avec `{game}`, `{creator}`, `{placeId}` et `{jobId}`.
- **Preset performance** : un clic pour couper le MSAA et lancer Roblox en priorité haute.
- **Fast Flags** : les réglages de rendu encore autorisés par Roblox, plus un éditeur pour le reste.
- **Mods** : ancien curseur, anciens sons, polices et emojis.
- **Infos serveur** : la région du serveur sur lequel tu joues et l'historique de tes parties.
- **Interface** : thème clair ou sombre, une trentaine de langues, et de la neige qui suit le taux de rafraîchissement de ton écran.

Aerostrap n'envoie aucune statistique, nulle part.

## Installation

1. Télécharge `Aerostrap.exe` depuis la [dernière release](https://github.com/noa15235/aerostrap/releases/latest).
2. Lance-le et suis l'installation.
3. C'est tout. Les liens « Jouer » du site passent maintenant par Aerostrap.

Il faut Windows 10 ou 11 et le [.NET 6 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/6.0).

Si Roblox est déjà installé depuis le site ou le Microsoft Store, désinstalle-le. Sinon il reprend les liens « Jouer » à chaque mise à jour et Aerostrap ne s'ouvre plus.

## Aperçu

<p align="center">
  <img src="Images/settings.png" width="80%" alt="Paramètres du moteur">
</p>

<p align="center">
  <img src="Images/integrations.png" width="49%" alt="Intégrations">
  <img src="Images/appearance.png" width="49%" alt="Apparence">
</p>

## Compiler

Il faut le [SDK .NET 6](https://dotnet.microsoft.com/download/dotnet/6.0).

```
git clone https://github.com/noa15235/aerostrap
cd aerostrap
dotnet publish Aerostrap/Aerostrap.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true
```

L'exécutable se retrouve dans `Aerostrap/bin/Release/net6.0-windows/win-x64/publish`.

## Un problème ?

Ouvre une [issue](https://github.com/noa15235/aerostrap/issues) en expliquant ce qui se passe. Joins le dernier fichier de `%LocalAppData%\Aerostrap\Logs`, ça aide beaucoup.

## Licence

[MIT](LICENSE)
