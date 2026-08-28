# ModularPipelines Development Instructions

**ALWAYS follow these instructions first and only fallback to additional search and context gathering if the information here is incomplete or found to be in error.**

ModularPipelines is a .NET framework that allows you to define CI/CD pipelines in C# using a module-based architecture with dependency management and parallel execution.

## Prerequisites and Setup

Install .NET SDK 10.0.302 (REQUIRED by global.json):
```bash
curl -sSL https://dotnet.microsoft.com/download/dotnet/scripts/v1/dotnet-install.sh | bash /dev/stdin --version 10.0.302
export PATH="$HOME/.dotnet:$PATH"
```

For documentation work, use a Node.js version allowed by `docs/package.json` (CI uses Node.js 24) and install yarn.

## Building the Code

**CRITICAL: NEVER CANCEL BUILD COMMANDS. Set timeout to 180+ seconds for individual builds.** (Agents should not run the full build pipeline at all - see below.)

**The default solution is lightweight.** `ModularPipelines.slnx` is the core-library
solution (core framework, Cmd, source generator, and analyzers only), so building it is
fast. Each tool/CLI integration has its own solution. Build the core solution or a single
tool solution for routine work.

**⛔ DO NOT build `ModularPipelines.All.slnx` or run the build pipeline
(`dotnet run` in `src/ModularPipelines.Build`).** Those compile 60+ projects at once and
will likely exhaust memory and lag or crash the machine. They are CI's responsibility, not
an agent's. Build only the core solution or the specific tool solution you are changing.
Only build `ModularPipelines.All.slnx` if a human explicitly requests a full build.

Build commands in order of complexity and timing:

1. **Build Core Library (DEFAULT, fastest)**:
```bash
# Core framework, Cmd, source generator, and analyzers only.
dotnet build ModularPipelines.slnx -c Release
```

2. **Build a single tool integration** (when working on one tool):
```bash
dotnet build src/ModularPipelines.Docker/ModularPipelines.Docker.slnx -c Release
```

3. **Build Analyzers** (22 seconds):
```bash
dotnet build ModularPipelines.Analyzers.slnx -c Release
```

4. **Build Examples** (80 seconds):
```bash
dotnet build ModularPipelines.Examples.slnx -c Release
```

**Never run as an agent (CI only - will likely crash the machine):**
```bash
# Full 60+ project solution:
#   dotnet build ModularPipelines.All.slnx -c Release
# Full build pipeline (builds ALL solutions + full test suite):
#   cd src/ModularPipelines.Build && dotnet run -c Release --framework net10.0
```
Note: When CI runs the build pipeline it may fail in a development environment due to
missing CI environment variables (like origin/main branch). This is expected. The pipeline
includes test execution with code coverage.

## Running Tests

**CRITICAL: NEVER CANCEL TEST COMMANDS. Set timeout to 300+ seconds.**

Run unit tests (95 seconds):
```bash
dotnet test test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj -c Release
```

Tests use TUnit framework. Some tests may fail in non-CI environments due to missing environment variables - this is expected.

## Code Formatting and Linting

Target the solution you built - `ModularPipelines.slnx` (core) for core changes, or the
relevant tool solution. Do not format `ModularPipelines.All.slnx` as an agent.

**Format verification** (70 seconds):
```bash
dotnet format ModularPipelines.slnx --verify-no-changes --severity info
```

**Fix formatting issues** (65 seconds):
```bash
dotnet format ModularPipelines.slnx
```

**Fix whitespace only** (18 seconds):
```bash
dotnet format ModularPipelines.slnx whitespace
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
- `ModularPipelines.slnx` - lightweight core-library solution (core framework, Cmd, source generator, analyzers). **Build this by default.**
- `src/ModularPipelines.<Tool>/ModularPipelines.<Tool>.slnx` - one solution per tool/CLI integration. Build the specific tool solution when working on it.
- `ModularPipelines.All.slnx` - full solution with all 60+ projects. **CI-only** - do not build as an agent; it can exhaust memory and crash the machine.
- `ModularPipelines.Analyzers.slnx` / `ModularPipelines.Examples.slnx` - analyzer and example solutions.

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
- Implement `ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)`
- Access other module results with `await context.GetModule<TModule>()`

**Pipeline Context:**
- `IModuleContext` provides access to all tools and services
- Tool extensions: `context.DotNet()`, `context.Git()`, `context.Docker()`, etc.
- Full dependency injection support

**Host Pattern:**
- Use `Pipeline.CreateBuilder(args)` to bootstrap
- Register modules with `builder.AddModule<T>()`
- Configure services through `builder.Services`
- Run with `await builder.RunAsync()`

## Validation Scenarios

**After making changes, ALWAYS:**
1. Build the affected solution - `ModularPipelines.slnx` (core) for core changes, or the specific tool solution. **Never build `ModularPipelines.All.slnx` or run the build pipeline** to validate - they compile 60+ projects and can crash the machine; let CI do the full build.
2. Run formatting on what you built, e.g. `dotnet format ModularPipelines.slnx`
3. Run tests for affected areas (the specific test project - not the full pipeline)
4. If modifying the build pipeline, build just its project to check it compiles: `dotnet build src/ModularPipelines.Build/ModularPipelines.Build.csproj -c Release`. Do not execute the pipeline (`dotnet run`) as an agent - let CI run it end to end.

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
- Ensure .NET 10.0.302 SDK is installed and in PATH
- Check `global.json` for required SDK version

**Test Failures:**
- Some tests expect CI environment variables and may fail locally
- Focus on tests related to your changes
- 3 out of 376 test failures is normal in development environment

**Formatting Issues:**
- Run `dotnet format ModularPipelines.slnx` to fix most issues
- Some analyzers cannot be auto-fixed (like RS1038)
- Check `.editorconfig` for style requirements

**Pipeline Failures:**
- The build pipeline expects CI environment (GitHub Actions)
- Module `ChangedFilesInPullRequestModule` will fail without origin/main branch
- This is expected in development environment

**NEVER use shorter timeouts than specified above. Build and test operations can legitimately take the full time specified.**
