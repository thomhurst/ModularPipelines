# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Common Development Commands

### Build Commands

The solution is deliberately split so the default build is fast. `ModularPipelines.slnx`
is now the **lightweight core-library solution** (core framework, Cmd, source generator,
and analyzers only). Each tool/CLI integration has its **own** solution next to it, and
`ModularPipelines.All.slnx` aggregates everything for full validation.

> [!CAUTION]
> **Agents: do NOT build `ModularPipelines.All.slnx`, and do NOT run the build pipeline
> (`dotnet run` in `src/ModularPipelines.Build`).** Compiling 60+ projects at once is
> extremely memory- and CPU-heavy and will likely lag or crash the machine. Those are
> CI's job. For local/agent work, build **only** `ModularPipelines.slnx` (core) or a
> single tool's own solution. Reach for `ModularPipelines.All.slnx` only when a human
> explicitly asks for a full build and the machine can handle it.

> [!IMPORTANT]
> **Agents: run every local `dotnet` command through
> `scripts/Invoke-AgentDotNet.ps1`; never invoke `dotnet` directly.** The guard
> disables reusable MSBuild/Roslyn servers, uses below-normal priority, and kills
> the entire process tree if it exceeds 10 minutes or 2 GB by default. Exit code
> `124` means timeout; `137` means memory limit. Do not raise either limit and retry
> automatically—record the local validation limit and let CI run the expensive check.
> Set the shell/tool timeout at least 30 seconds longer than `-TimeoutSeconds` so the
> guard, rather than the outer shell, owns cleanup.

```powershell
# Core build
pwsh scripts/Invoke-AgentDotNet.ps1 -SingleNode `
  -DotNetArguments @('build', 'ModularPipelines.slnx', '-c', 'Release')

# Core build including ModularPipelines.UnitTests
pwsh scripts/Invoke-AgentDotNet.ps1 -SingleNode `
  -DotNetArguments @('build', 'ModularPipelines.Tests.slnf', '-c', 'Release')

# Tool-specific build
pwsh scripts/Invoke-AgentDotNet.ps1 -SingleNode `
  -DotNetArguments @(
    'build',
    'src/ModularPipelines.Docker/ModularPipelines.Docker.slnx',
    '-c',
    'Release'
  )
```

```bash
# Human/manual equivalents:

# DEFAULT: build the core library (fast). This is ModularPipelines.slnx - the core
# framework, Cmd, source generator, and analyzers only.
dotnet build ModularPipelines.slnx -c Release

# Build the core library and its unit-test project without building the full solution.
dotnet build ModularPipelines.Tests.slnf -c Release

# Working on a tool/CLI integration? Build that tool's own solution.
# It includes the tool's matching unit-test project when one exists.
dotnet build src/ModularPipelines.Docker/ModularPipelines.Docker.slnx -c Release

# Other solutions - only when working on those areas.
dotnet build ModularPipelines.Examples.slnx -c Release
dotnet build ModularPipelines.Analyzers.slnx -c Release

# DANGER (CI only): the full 60+ project solution and the full build pipeline.
# Do NOT run these as an agent - they can exhaust memory and crash the machine.
# dotnet build ModularPipelines.All.slnx -c Release            # everything at once
# cd src/ModularPipelines.Build && dotnet run -c Release --framework net10.0  # builds ALL solutions + full test suite
```

**What `ModularPipelines.slnx` (core) contains:**
- `src/ModularPipelines` - core framework
- `src/ModularPipelines.Cmd` - base command execution
- `src/ModularPipelines.SourceGenerator` - source generator
- `src/ModularPipelines.Analyzers` + `.CodeFixes` - analyzers referenced by the core

Every tool/CLI integration (Docker, DotNet, Git, Helm, Terraform, Azure, AWS, etc.) is a
separate package whose options are largely auto-generated, and each has its own
`src/ModularPipelines.<Tool>/ModularPipelines.<Tool>.slnx`. Each tool solution includes
its matching unit-test project when one exists. Build the specific tool solution when
working on it. `ModularPipelines.Tests.slnf` provides the equivalent test-building loop
for the lightweight core. `ModularPipelines.All.slnx` exists for CI's full build only — see
the caution above; agents should not build it.

### Running Tests
```powershell
# PREFER: run only the test project relevant to your change. This avoids
# building the whole solution and every tool integration.
pwsh scripts/Invoke-AgentDotNet.ps1 `
  -DotNetArguments @(
    'run',
    '--project',
    '<path-to-test-project>',
    '--framework',
    'net10.0',
    '--',
    '--coverage',
    '--coverage-output-format',
    'cobertura'
  )
```

> [!CAUTION]
> Do not run the full test suite via the build pipeline (`dotnet run` in
> `src/ModularPipelines.Build`) as an agent - it builds every solution and can crash the
> machine. Run the single relevant test project instead; let CI run the full suite.

### Code Formatting
```powershell
# Format code (automatically done in CI). Target the solution you built - the
# core solution (ModularPipelines.slnx) for core changes, or the relevant tool solution.
pwsh scripts/Invoke-AgentDotNet.ps1 `
  -DotNetArguments @('format', 'ModularPipelines.slnx')
pwsh scripts/Invoke-AgentDotNet.ps1 `
  -DotNetArguments @('format', 'ModularPipelines.slnx', 'whitespace')

# Verify formatting without changes
pwsh scripts/Invoke-AgentDotNet.ps1 `
  -DotNetArguments @(
    'format',
    'ModularPipelines.slnx',
    '--verify-no-changes',
    '--severity',
    'info'
  )
```

## High-Level Architecture

### Core Concepts

**Module-Based Pipeline Architecture**: ModularPipelines uses a module system where each unit of work is a self-contained `Module<T>` class. Modules can depend on each other via `[DependsOn<T>]` attributes, creating a dependency graph that automatically parallelizes work.

**Key Components**:

1. **Module System** (`src/ModularPipelines/Modules/`):
   - Base class: `Module<T>` where T is the return type
   - Modules execute via `ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)`
   - Dependencies declared with `[DependsOn<TModule>]` attributes
   - Skip conditions via `ShouldSkip()` method
   - Retry policies configurable per module

2. **Module Context** (`IModuleContext`):
   - Central interface providing access to all tools and services
   - Includes: file system operations, command execution, logging, Git info
   - Discoverable tool properties (e.g., `context.Tools.DotNet`, `context.Tools.Git`)

3. **Host Pattern** (`PipelineHostBuilder`):
   - Built on Microsoft Generic Host
   - Full dependency injection support
   - Configuration via `appsettings.json`, user secrets, environment variables
   - Module registration and pipeline execution

4. **Tool Integrations**:
   - Each tool (DotNet, Git, Docker, etc.) has its own package
   - Strongly-typed options classes for CLI commands
   - Fluent API for building complex commands
   - Automatic secret obfuscation in logs

5. **Code Generation**:
   - Tool options classes (e.g., `GitAddOptions`, `DotNetBuildOptions`, `DockerRunOptions`) are **auto-generated**
   - Generator located at: `tools/ModularPipelines.OptionsGenerator/`
   - **Never hand-tweak auto-generated options in a PR** - changes will be overwritten
   - Fix generated-options problems in the scraper or generator, not in generated output
   - Prefer fixes in generic generator logic so every tool benefits; add tool-specific logic only when a generic fix is not possible
   - Current tool output is the sole source of truth for generated options, even when regeneration causes breaking API changes
   - Do not preserve earlier generated APIs with handwritten shims, aliases, obsolete forwarding members, or manual extension files
   - To modify generated options behavior, update the scraper or generator so the result represents the current tool
   - Generated files have `[ExcludeFromCodeCoverage]` attribute

### Project Structure

- `src/ModularPipelines/` - Core framework
- `src/ModularPipelines.*` - Tool-specific integrations (DotNet, Git, Docker, Azure, AWS, etc.)
- `src/ModularPipelines.Build/` - This project's build pipeline
- `test/*.UnitTests/` - Unit test projects
- `docs/` - Docusaurus documentation site

### Build Pipeline Organization

The build pipeline (`src/ModularPipelines.Build/`) demonstrates best practices:
- Separate modules for each build step
- Dependency management between modules
- Conditional execution based on environment (development vs CI)
- Integration with GitHub Actions, NuGet publishing, code coverage

### Key Patterns

1. **Strong Typing**: All data passed between modules is strongly typed
2. **Parallel Execution**: Work runs concurrently unless dependencies prevent it
3. **Dependency Injection**: Full DI support for services and configuration
4. **Hooks**: Before/after module execution hooks
5. **Requirements**: Pipeline requirements validation (OS, permissions, etc.)
6. **Skip Logic**: Modules can be skipped based on custom conditions
7. **Progress Reporting**: Real-time console progress with parallel execution visualization

### Development Workflow

1. Create modules by inheriting from `Module<T>`
2. Define dependencies with `[DependsOn<T>]` attributes
3. Implement `ExecuteAsync` method
4. Register modules in `Program.cs` using `AddModule<T>()`
5. Configure services and settings via DI
6. Run pipeline with `dotnet run`

### Testing Approach

- Unit tests use the project's own pipeline system
- Tests run with code coverage collection enabled
- Coverage reports uploaded to Codacy and Codecov
- Test projects identified by "*UnitTests.csproj" pattern
- remember the correct filter syntax
