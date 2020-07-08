# Orbis-Suite
A suite of tools used for developing things on a jailbroken PS4. Aswell as an included DLL for creating 3rd party C# applications using the OrbisLib API.

![UI](https://pbs.twimg.com/media/EDNZOwDVUAAoeJF?format=jpg&name=4096x4096)

# TODO List
###### Windows Service
- [ ] Implement into new project and recode to c++
- [ ] Manage Callbacks
- [ ] Signal hang to C# apps
- [ ] Manage Socket/UART Output
- [ ] Signal System Status to C# apps
###### Orbis Neighborhood
- [x] Implement into new project
- [ ] Implement Functionality
###### Debugger
- [ ] Implement into new project
- [ ] Implement and test Breakpoints
- [ ] Implement Thread control and state
- [ ] Implement Exeption codes
- [ ] Implement and test Watchpoints	
###### Console Output
- [ ] Implement into new project
###### Module Manager
- [ ] Implement into new project
###### SPRX Helper
- [ ] Implement into new project
###### Taskbar App
- [ ] Implement into new project
###### C++ DLL Helper
- [ ] Implement into new project
- [ ] Clean up and organize code
###### OrbisLib DLL
- [ ] Finish Implementing old features and cleaning up code
- [ ] Add More error handling 
- [ ] Move more functionality to the c++ DLL for better effeciency
###### Orbis Kernel ELF
- [ ] Impliment into github project / new repository
- [ ] Clean up unessisary code
- [ ] Clean up Packets and make naming more consistant
- [ ] clean up shellcode for userland processes
- [ ] clean up call back thread
- [ ] clean up connection management
- [ ] clean up process management
###### Future Features
- [ ] Callback for Remote Procedure Calls
- [ ] module loader for games (auto load modules for games on start up with config to tell what to load)
- [ ] module loader for boot (auto load kernel modules on boot. or maybe like a vsh sprx)
- [ ] Remote video feed of screen and or screen shot feature


# Features
###### API for remote target control
- [x] Memory Read/ Write
- [ ] Remote Procedure Calls
- [x] Loading / Unloading SPRX Libraries to Userland Processes
- [x] Loading ELF's to Userland Processes
- [x] Jail/Unjail Userland Processes
###### Comprehensive Debugger
- [x] Breakpoints / Watchpoints
- [x] Register Management
- [x] Memory view and dumper
- [x] Instruction Disassembly
- [ ] StopCode Decoding
- [x] Process Management (Stop/Start/Single Step/Step Over/Step Out)
- [ ] Process Thread Management
- [x] Customizable Debugging environment
###### Module Management
- [x] Load/Reload/Unload SPRX Libraries to Userland Process
- [x] Load ELF to Userland Process
- [ ] Dumping Userland Processes and Libraries
- [x] List of loaded Libraries in Userland Process
- [x] File browser
###### Taskbar Application
- [x] Add/Manage saved Targets
- [x] Open Various Orbis Suite Apps
- [ ] Manage Power state of console
- [x] Load/auto load payload
- [x] Set Taskbar app to auto load on windows boot
###### Console Output
- [x] Read UART/Socket Prints from remote Target
###### Orbis Neighborhood
- [x] GUI for managing saved Targets
###### SPRX Helper
- [x] Easily impliment automatic fsign and ftp sprx for debugging
###### C# DLL for easy 3rd party application Development using API
- [x] Full access to API calls and features
- [ ] Remote Procedure calls
