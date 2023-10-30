# OrbisLib
The C# interface library for interactions with the orbis API.

# Build Instructions
1. Ensure that .NET 7.0 SDK is installed.
2. Build the submodule dependancies found in the "(SolutionDir)\External" Folder.
3. Restore the nuget packages.
4. Fix broken COM dependancy paths if needed.
5. Build the project.

# Nuget Dependancies
- sqlite-net-pcl
- Ftp.dll
- Google.Protobuf
- H.Pipes
- Microsoft.Extensions.Logging.Abstractions
- System.Data.SQLite
- System.Json

# External Submodule Dependancies
- SimpleUI
- WpfHexEditorControl