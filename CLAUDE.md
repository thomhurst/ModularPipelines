# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Common Development Commands

### Build Commands

The solution is deliberately split so the default build is fast. `ModularPipelines.sln`
is now the **lightweight core-library solution** (core framework, Cmd, source generator,
and analyzers only). Each tool/CLI integration has its **own** solution next to it, and
`ModularPipelines.All.sln` aggregates everything for full validation.

```bash
# DEFAULT: build the core library (fast). This is ModularPipelines.sln - the core
# framework, Cmd, source generator, and analyzers only.
dotnet build ModularPipelines.sln -c Release

# Working on a tool/CLI integration? Build that tool's own solution.
# (Most tool integrations have auto-generated options - see the code generation notes below.)
dotnet build src/ModularPipelines.Docker/ModularPipelines.Docker.sln -c Release

# FULL solution (60+ projects, slow) - only when your change spans many tool
# integrations or you specifically need to validate everything at once.
dotnet build ModularPipelines.All.sln -c Release

# Other solutions - only when working on those areas.
dotnet build ModularPipelines.Examples.sln -c Release
dotnet build ModularPipelines.Analyzers.sln -c Release

# Run the build pipeline (from src/ModularPipelines.Build)
# This builds ALL solutions and runs the full test suite - it is the slow,
# comprehensive path used by CI. Do not run it for routine core changes.
cd src/ModularPipelines.Build
dotnet run -c Release --framework net10.0
```

**What `ModularPipelines.sln` (core) contains:**
- `src/ModularPipelines` - core framework
- `src/ModularPipelines.Cmd` - base command execution
- `src/ModularPipelines.SourceGenerator` - source generator
- `src/ModularPipelines.Analyzers` + `.CodeFixes` - analyzers referenced by the core

Every tool/CLI integration (Docker, DotNet, Git, Helm, Terraform, Azure, AWS, etc.) is a
separate package whose options are largely auto-generated, and each has its own
`src/ModularPipelines.<Tool>/ModularPipelines.<Tool>.sln`. Build the specific tool
solution when working on it, or `ModularPipelines.All.sln` to build everything.

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
# core solution (ModularPipelines.sln) for core changes, or the relevant tool solution.
dotnet format ModularPipelines.sln
dotnet format ModularPipelines.sln whitespace

# Verify formatting without changes
dotnet format ModularPipelines.sln --verify-no-changes --severity info
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
