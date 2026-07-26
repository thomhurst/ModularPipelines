# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Common Development Commands

### Build Commands

> [!IMPORTANT]
> **Default to the core solution.** The full `ModularPipelines.sln` contains 60+
> projects (the core framework plus every tool/CLI integration), so building it is
> slow and memory-hungry. Most work only touches the core framework, so build the
> lightweight `ModularPipelines.Core.sln` instead. Only build the full solution, the
> examples/analyzers solutions, or an individual tool project when you are
> **explicitly working on those projects**.

```bash
# DEFAULT: build the core library only (fast) - core framework, Cmd,
# source generator, and analyzers. Use this for most changes.
dotnet build ModularPipelines.Core.sln -c Release

# Working on a single tool/CLI integration? Build just that project.
# (Most tool integrations have auto-generated options - see the code generation notes below.)
dotnet build src/ModularPipelines.Docker/ModularPipelines.Docker.csproj -c Release

# FULL solution (60+ projects, slow) - only when your change spans many
# tool integrations or you specifically need to validate everything.
dotnet build ModularPipelines.sln -c Release

# Other solutions - only when working on those areas.
dotnet build ModularPipelines.Examples.sln -c Release
dotnet build ModularPipelines.Analyzers.sln -c Release

# Run the build pipeline (from src/ModularPipelines.Build)
# This builds ALL solutions and runs the full test suite - it is the slow,
# comprehensive path used by CI. Do not run it for routine core changes.
cd src/ModularPipelines.Build
dotnet run -c Release --framework net10.0
```

**What `ModularPipelines.Core.sln` contains:**
- `src/ModularPipelines` - core framework
- `src/ModularPipelines.Cmd` - base command execution
- `src/ModularPipelines.SourceGenerator` - source generator
- `src/ModularPipelines.Analyzers` + `.CodeFixes` - analyzers referenced by the core

Everything else (Docker, DotNet, Git, Helm, Terraform, Azure, AWS, etc.) is a
tool/CLI integration whose options are largely auto-generated. These are excluded
from the core solution - add the specific project you need, or use the full solution.

### Running Tests
```bash
# PREFER: run only the test project relevant to your change. This avoids
# building the whole solution and every tool integration.
dotnet run --project <path-to-test-project> --framework net10.0 -- --coverage --coverage-output-format cobertura

# Run all unit tests via the build pipeline with coverage.
# This is the slow, comprehensive path (builds all solutions) - use it for a
# full verification pass, not for routine iteration.
cd src/ModularPipelines.Build
dotnet run -c Release --framework net10.0
```

### Code Formatting
```bash
# Format code (automatically done in CI). Target the solution you built - the
# core solution for core changes, or the relevant tool project otherwise.
dotnet format ModularPipelines.Core.sln
dotnet format ModularPipelines.Core.sln whitespace

# Verify formatting without changes
dotnet format ModularPipelines.Core.sln --verify-no-changes --severity info
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
   - Extensions for each tool integration (e.g., `context.DotNet()`, `context.Git()`)

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
   - **Do not modify generated options classes directly** - changes will be overwritten
   - To modify options behavior, update the generator or add manual extension files
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
