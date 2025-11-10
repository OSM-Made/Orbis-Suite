![Header](/Assets/Header.png?raw=true)
#
The next version of Orbis Suite completely rewritten with a new GUI. Now launchable through a homebrew app gone is the need for a payload! Fully featured suite of tools to help aid in developing and debugging on the PS4 console. 

# Features
- Library / API for integrating with the Target Console.
- User friendly and reactive UI.
- Memory **Peek n Poker**
- **Library Manager** (can be used to load custom Libraries!)
- **Console Output** (Nice filtered view of the console output.)
- **Neighborhood** (Used for easy interaction and saving of Targets.)
- Custom UI Elements on the Target Console.

# NeighborHood
![](/Assets/Neighborhood1.png)
![](/Assets/Neighborhood2.png)
![](/Assets/Neighborhood3.png)

# Build Requirements
- PS4 Official SDK
- [Wix Toolset 3.11](https://github.com/wixtoolset/wix3/releases/tag/wix3112rtm)
- [.NET Framework 4.8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48)
- [.NET 7.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

# Build Instructions
1. Ensure that the SDK is installed for .NET 7 & .NET Framework 4.8.
2. Enrure that Wix Toolset 3.11 is installed & configured.
3. Build dependencies found in the `\External` folder. 
4. Restore nuget packages.
5. Build the `OrbisAPI.sln` using the instructions found [here](/Playstation/README.md).
6. Build the `Orbis Suite 3.0.sln` using **Visual Studio**.

# Special Thanks
- Zenco
- ![Alex / Skiff](https://github.com/skiff)
- ![Flatz](https://github.com/flatz)
- ![iMoD1998](https://github.com/iMoD1998)
- ![ZzReApErzZ](https://github.com/Peribunt)
- ![Synful](https://github.com/Synful)
- ![SiSTR0](https://github.com/SiSTR0)
- ![LightningMods](https://github.com/LightningMods)
- ![kiwidoggie](https://github.com/kiwidoggie)
- ![Al-Azif](https://github.com/Al-Azif)
- Many More I may forget!
