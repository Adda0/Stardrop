# StardropForked

This is the initial Flatpak version for my fork of Stardrop with some custom changes and added support for Flatpak.

It is an alpha version and there are still some parts which do not work for one reason or another, but in general the Flatpak seems to be in a working state. Adding mods, checking for updates, selecting mods for your mod list, and updating your profiles works.

Feel free to test it and give me a report on how well it works for you. First, read the [README.md](https://github.com/Adda0/Stardrop/blob/flatpak/README.md) on how to use the Flatpak. Direct all issues and bug reports to [my fork of Stardrop/issues](https://github.com/Adda0/Stardrop/issues). It should be safe to use, but there are no guarantees. The version is still a testing alpha version. I would highly appreciate it if some of you could give it a try (any Linux users, especially those on Steam Deck, and anyone else with Flatpak support on their system) and report back what are the results.

When the Flatpak matures enough, and all necessary features from my fork are merged into the upstream [Stardrop](https://github.com/Floogen/Stardrop/), I hope to redirect the Flatpak from my fork to there. But for now, the Flatpak is based on my fork.

## Installing and running StardropForked Flatpak

To install the Flatpak, run in your terminal the following command:
``` sh
flatpak install io.github.Adda0.Stardrop
```

To run the StardropForked, run:
``` sh
flatpak run io.github.Adda0.Stardrop
```
or find the flatpak in your system application program runner, it should be named `StardropForked`.

Feel free to try to use the Flatpak as you see fit. Do not forget to configure the program settings (your Stardew Valley folder with installed SMAPI, yout mods folder, etc.) when the application starts.

Stardrew Valley folder will probably be located in `~/.local/share/Steam/steamapps/common/Stardew Valley` if installed through Steam, but it might not be the case.
The mods folder will probably be `~/.local/share/Steam/steamapps/common/Stardew Valley/Mods`.
I suggest installing Stardrop installed mods in the same directory, that is, `~/.local/share/Steam/steamapps/common/Stardew Valley/Mods`. But if your want to install elsewhere, feel free to and let me know if everything (especially the grouping of mods) works correctly.

Remember that you in this version, you have to click `Save Changes` button at top right to save the changes made to the mod list. Keep in mind that adding mods or updating the existing ones will reset the changes made. Always save changes before adding/updating mods.

Remember to never run Stardew Valley through StardropForked Flatpak (the pufferchick icon at top right). It works, but running a game through Flatpak might introduce some performance slowdowns which are definitely unwanted. Instead, run Stardew Valley from a new terminal with the following command (for now, better options to run the game are on the list of things to work out): (NOTE: If this command fails with an error saying that no usable version of `libssl` was found, just run Stardew through the Pufferchick button. The performance impact should be minimal, if any, anyway.)
``` sh
~/.local/share/Steam/steamapps/common/Stardew\ Valley/StardewModdingAPI --mods-path ~/.var/app/io.github.Adda0.Stardrop/config/Stardrop/Data/Selected\ Mods

```
Modify the command to point to wherever your StardewModdingAPI is located.

# Original Project Description
 
Stardrop is an open-source, cross-platform mod manager for the game [Stardew Valley](https://www.stardewvalley.net/). It is built using the Avalonia UI framework.

Stadrop utilizes [SMAPI (Stardew Modding API)](https://smapi.io/) to simplify the management and update checking for all applicable Stardew Valley mods.

Profiles are also supported, allowing users to have multiple mod groups for specific gameplay or multiplayer sessions.

# Getting Started

See the [GitBook pages](https://floogen.gitbook.io/stardrop/) for detailed documentation on how to install, update and use Stardrop.

## Downloading Stardrop

See the [release page](https://github.com/Floogen/Stardrop/releases/latest) for the latest builds.

# Credits
## Translations
Stardrop has been generously translated into several languages by the following users:

* **Chinese** - guanyintu, PIut02, Z2549, CuteCat233
* **French** - xynerorias
* **German** - Schn1ek3
* **Hungarian** - martin66789
* **Italian** - S-zombie
* **Portuguese** - aracnus
* **Russian** - Rongarah
* **Spanish** - Evexyron, Gaelhaine
* **Thai** - ellipszist
* **Turkish** - KediDili
* **Ukrainian** - burunduk, ChulkyBow

# Gallery

![](https://imgur.com/WdjwfnG.gif)

![](https://imgur.com/kalsOjS.gif)
