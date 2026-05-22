# Copilot instructions — dvplugins

Purpose: short, actionable guidance for Copilot sessions operating on this repo.

## Quick build / pack commands
- Build Core project:
  dotnet build .\dvplugins.Core\dvplugins.Core.csproj -c Release
- Build Plugins project (used by VS Code task):
  dotnet build .\dvplugins.Plugins\dvplugins.Plugins.csproj -c Release
- Pack Plugins into a NuGet package (produces .nupkg in bin/Debug or output dir):
  dotnet pack .\dvplugins.Plugins\dvplugins.Plugins.csproj -c Debug -o .\bin\Debug
- VS Code tasks (dvplugins.Plugins): `build` runs dotnet build on dvplugins.Plugins.csproj; `watch` runs `dotnet watch run`.
- Open solution in Visual Studio: open `dvplugins.slnx` (solution file used by the repo).

## Tests & linting
- There are no test projects in the repo. No repository-level linter or CI linter config detected. Several GeneratedMSBuildEditorConfig.editorconfig files exist in obj/ output but no explicit style/lint tooling is configured.

## High-level architecture (big picture)
- Two main projects:
  - dvplugins.Core: library with plugin abstractions and helpers (PluginBase.cs, LocalPluginContext.cs, ILocalPluginContext.cs).
  - dvplugins.Plugins: concrete plugin implementations (ExamplePlugin.cs), references dvplugins.Core and is configured to produce a NuGet package containing the plugin and its dependent assemblies.
- Both projects target .NET Framework 4.6.2 (net462) to be compatible with Dataverse/Power Platform.
- The Plugins project imports PowerApps/Power Platform MSBuild props and targets (see PowerAppsTargetsPath imports in the csproj). Building in CI or headless environments may require those build targets or Visual Studio workloads installed.
- Assemblies are strong-name signed using .snk files checked into the project roots (dvplugins.Core.snk, dvplugins.Plugins.snk).

## Key conventions and repo-specific patterns
- Dataverse guardrails (see .gemini skill):
  - Thread-safety: do not store service instances in class-level fields. Create a LocalPluginContext inside the plugin Execute method and use it per-execution.
  - Use block namespaces (C# 7.3) — file-scoped namespaces are not used.
  - Assembly signing is required; keep and reference the .snk files and the SignAssembly / AssemblyOriginatorKeyFile entries in the csproj.
- Packaging: Plugins are distributed as NuGet packages (Dependent Assemblies). Use `dotnet pack`/`msbuild /t:Pack` to create .nupkg in bin/Debug.
- PowerApps MSBuild integration: dvplugins.Plugins.csproj imports Microsoft.PowerApps.VisualStudio.Plugin.props/targets when available — local devs typically build inside Visual Studio with the Power Platform tooling installed.
- VS Code: dvplugins.Plugins contains a .vscode/tasks.json providing `build` and `watch` tasks (dotnet build / dotnet watch run) — follow this for quick iteration.

## Important files to check for context
- dvplugins.Core\PluginBase.cs
- dvplugins.Core\LocalPluginContext.cs
- dvplugins.Core\ILocalPluginContext.cs
- dvplugins.Plugins\ExamplePlugin.cs
- dvplugins.Plugins\.vscode\tasks.json
- .gemini/skills/dataverse-plugin-setup/SKILL.md (contains packaging and thread-safety rules)


If anything here should be expanded (e.g., adding CI commands, single-test guidance, or mapping to existing build agents), say which area to cover and Copilot will update this file.