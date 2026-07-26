# ModularPipelines Development Instructions

**ALWAYS follow these instructions first and only fallback to additional search and context gathering if the information here is incomplete or found to be in error.**

ModularPipelines is a .NET framework that allows you to define CI/CD pipelines in C# using a module-based architecture with dependency management and parallel execution.

## Prerequisites and Setup

Install .NET SDK 9.0.301 (REQUIRED by global.json):
```bash
curl -sSL https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh | bash /dev/stdin --version 9.0.301
export PATH="$HOME/.dotnet:$PATH"
```

Install .NET 8.0 SDK (REQUIRED for build pipeline execution):
```bash
curl -sSL https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh | bash /dev/stdin --version 8.0.118
```

For documentation work, ensure Node.js 20+ is available and yarn is installed.

## Building the Code

**CRITICAL: NEVER CANCEL BUILD COMMANDS. Set timeout to 180+ seconds for individual builds, 600+ seconds for the build pipeline.**

**DEFAULT TO THE CORE SOLUTION.** The full `ModularPipelines.sln` has 60+ projects
(the core framework plus every tool/CLI integration) and is slow and memory-hungry to
build. Build `ModularPipelines.Core.sln` for routine work, and only build the full
solution, the examples/analyzers solutions, or an individual tool project when you are
**explicitly working on those projects**.

Build commands in order of complexity and timing:

1. **Build Core Library (DEFAULT, fastest)**:
```bash
# Core framework, Cmd, source generator, and analyzers only.
dotnet build ModularPipelines.Core.sln -c Release
```

2. **Build a single tool integration** (when working on one tool):
```bash
dotnet build src/ModularPipelines.Docker/ModularPipelines.Docker.csproj -c Release
```

3. **Build Analyzers** (22 seconds):
```bash
dotnet build ModularPipelines.Analyzers.sln -c Release
```

4. **Build Main Solution** (35 seconds - 60+ projects, only when needed):
```bash
dotnet build ModularPipelines.sln -c Release
```

5. **Build Examples** (80 seconds):
```bash
dotnet build ModularPipelines.Examples.sln -c Release
```

6. **Run Build Pipeline** (150+ seconds - NEVER CANCEL, builds everything):
```bash
cd src/ModularPipelines.Build
dotnet run -c Release --framework net10.0
```
Note: The build pipeline may fail in development environment due to missing CI environment variables (like origin/main branch). This is expected. The pipeline includes test execution with code coverage.

## Running Tests

**CRITICAL: NEVER CANCEL TEST COMMANDS. Set timeout to 300+ seconds.**

Run unit tests (95 seconds):
```bash
dotnet test test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj -c Release
```

Tests use TUnit framework. Some tests may fail in non-CI environments due to missing environment variables - this is expected.

## Code Formatting and Linting

Target the solution you built - `ModularPipelines.Core.sln` for core changes, or the
relevant tool project/full solution otherwise.

**Format verification** (70 seconds):
```bash
dotnet format ModularPipelines.Core.sln --verify-no-changes --severity info
```

**Fix formatting issues** (65 seconds):
```bash
dotnet format ModularPipelines.Core.sln
```

**Fix whitespace only** (18 seconds):
```bash
dotnet format ModularPipelines.Core.sln whitespace
```

**ALWAYS run formatting before committing changes** or CI will fail.

## Documentation

**Install dependencies** (28 seconds):
```bash
cd docs
yarn install
```

**Build documentation** (30 seconds):
```bash
cd docs
yarn build
```

**Serve documentation locally**:
```bash
cd docs
yarn start
```

## Repository Structure

**Solutions:**
- `ModularPipelines.Core.sln` - lightweight core-library solution (core framework, Cmd, source generator, analyzers). **Build this by default.**
- `ModularPipelines.sln` - full solution with all 60+ projects including every tool integration. Slow; build only when needed.
- `ModularPipelines.Analyzers.sln` / `ModularPipelines.Examples.sln` - analyzer and example solutions.

**Core Framework:**
- `src/ModularPipelines/` - Core framework and base classes
- `src/ModularPipelines.Cmd/` - Base command execution
- `src/ModularPipelines.SourceGenerator/` - Source generator
- `src/ModularPipelines.*/` - Tool-specific integrations (Azure, AWS, Docker, Git, etc.); options are largely auto-generated. Excluded from the core solution.

**Build System:**
- `src/ModularPipelines.Build/` - The project's own build pipeline implementation
- `.github/workflows/dotnet.yml` - GitHub Actions workflow

**Testing:**
- `test/ModularPipelines.UnitTests/` - Main unit test project (uses TUnit)
- `test/ModularPipelines.TestHelpers/` - Test utilities

**Documentation:**
- `docs/` - Docusaurus-based documentation site

## Key Development Concepts

**Module System:**
- Inherit from `Module<T>` where T is the return type
- Use `[DependsOn<TModule>]` attributes for dependencies
- Implement `ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)`
- Access other module results with `await GetModule<TModule>()`

**Pipeline Context:**
- `IPipelineContext` provides access to all tools and services
- Tool extensions: `context.DotNet()`, `context.Git()`, `context.Docker()`, etc.
- Full dependency injection support

**Host Pattern:**
- Use `PipelineHostBuilder.Create()` to bootstrap
- Register modules with `.AddModule<T>()`
- Configure services with `.ConfigureServices()`

## Validation Scenarios

**After making changes, ALWAYS:**
1. Build the affected solution/project - `ModularPipelines.Core.sln` for core changes, or the specific tool project. Avoid building the full solution unless your change spans many projects.
2. Run formatting on what you built, e.g. `dotnet format ModularPipelines.Core.sln`
3. Run tests for affected areas
4. If modifying the build pipeline, test with: `cd src/ModularPipelines.Build && dotnet run -c Release --framework net10.0`

**For module development:**
1. Create a module inheriting from `Module<T>`
2. Add proper `[DependsOn<T>]` attributes
3. Register in Program.cs
4. Test the pipeline execution

**For tool integrations:**
1. Check existing tool integrations in `src/ModularPipelines.*/`
2. Build/test only the specific tool project you are working on (e.g. `dotnet build src/ModularPipelines.<Tool>/ModularPipelines.<Tool>.csproj`) - these are excluded from the core solution
3. Follow the pattern of strongly-typed options classes
4. Use fluent API builders for complex commands
5. Include secret obfuscation for sensitive parameters

## Common File Locations

**Configuration:**
- `global.json` - .NET SDK version requirements
- `Directory.Build.props` - MSBuild properties
- `Directory.Packages.props` - NuGet package versions
- `.editorconfig` - Code style settings

**Build Pipeline Modules:**
- `src/ModularPipelines.Build/Modules/` - Individual build steps
- `src/ModularPipelines.Build/Program.cs` - Pipeline configuration

**Examples:**
- `src/ModularPipelines.Examples/` - Usage examples

## Troubleshooting

**Build Issues:**
- Ensure .NET 9.0.301 SDK is installed and in PATH
- For build pipeline, also ensure .NET 8.0 runtime is available
- Check `global.json` for required SDK version

**Test Failures:**
- Some tests expect CI environment variables and may fail locally
- Focus on tests related to your changes
- 3 out of 376 test failures is normal in development environment

**Formatting Issues:**
- Run `dotnet format ModularPipelines.sln` to fix most issues
- Some analyzers cannot be auto-fixed (like RS1038)
- Check `.editorconfig` for style requirements

**Pipeline Failures:**
- The build pipeline expects CI environment (GitHub Actions)
- Module `ChangedFilesInPullRequestModule` will fail without origin/main branch
- This is expected in development environment

**NEVER use shorter timeouts than specified above. Build and test operations can legitimately take the full time specified.**