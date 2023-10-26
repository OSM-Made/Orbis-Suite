# OrbisAPIDaemon
API Interface for remote control of a jailbroken PS4 console.

# Build Instructions
1. Initialize all submodules in ``External`` and build the respective projects.
2. Copy the **'libGoldHEN.sprx'** from its submodule project to ``build/pkg`` and ``build/pkg/Daemons/ORBS30000``.
3. Copy the **'libKernelInterface.sprx'** from its submodule project to ``build/pkg/Daemons/ORBS30000``.
4. Build all of the projects in the Playstation folder.
5. Pack the ORBS30000 dir into a 7z achive so that the ORBS30000 is includeded in the archive.
6. Build the pkg using your choice of tool.
