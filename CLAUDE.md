# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Common Development Commands

### Build Commands
```bash
# Build the entire solution
dotnet build ModularPipelines.sln -c Release

# Build specific solution (analyzers, etc.)
dotnet build ModularPipelines.Analyzers.sln -c Release

# Run the build pipeline (from src/ModularPipelines.Build)
cd src/ModularPipelines.Build
dotnet run -c Release --framework net8.0
```

### Running Tests
```bash
# Run all unit tests (pattern: *UnitTests.csproj)
# Tests are executed via the build pipeline with coverage
cd src/ModularPipelines.Build
dotnet run -c Release --framework net8.0

# Run a single test project manually
dotnet run --project <path-to-test-project> --framework net9.0 -- --coverage --coverage-output-format cobertura
```

### Code Formatting
```bash
# Format code (automatically done in CI)
dotnet format
dotnet format whitespace

# Verify formatting without changes
dotnet format --verify-no-changes --severity info
```

## High-Level Architecture

### Core Concepts

**Module-Based Pipeline Architecture**: ModularPipelines uses a module system where each unit of work is a self-contained `Module<T>` class. Modules can depend on each other via `[DependsOn<T>]` attributes, creating a dependency graph that automatically parallelizes work.

**Key Components**:

1. **Module System** (`src/ModularPipelines/Modules/`):
   - Base class: `Module<T>` where T is the return type
   - Modules execute via `ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)`
   - Dependencies declared with `[DependsOn<TModule>]` attributes
   - Skip conditions via `ShouldSkip()` method
   - Retry policies configurable per module

2. **Pipeline Context** (`IPipelineContext`):
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