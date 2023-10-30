# OrbisNeighborHood

# Build Instructions
1. Ensure that .NET 7.0 SDK is installed.
2. Build the submodule dependancies found in the "(SolutionDir)\External" Folder.
3. Build the Project dependancies.
4. Restore the nuget packages.
5. Fix broken COM dependancy paths if needed.
6. Build the project.

# Nuget Dependancies
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Logging
- Microsoft.Extensions.Logging.Abstractions
- Microsoft.Extensions.Logging.Console
- Serilog.Extensions.Logging.File
- WpfAnimatedGif

# External Submodule Dependancies
- SimpleUI

# Project Dependancies
- OrbisLib
- OrbisSuiteCore