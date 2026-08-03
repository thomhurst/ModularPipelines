# ModularPipelines

**Write CI/CD pipelines in C#. Debug them locally. Ship with confidence.**

[![nuget](https://img.shields.io/nuget/v/ModularPipelines.svg)](https://www.nuget.org/packages/ModularPipelines/)

![Nuget](https://img.shields.io/nuget/dt/ModularPipelines) ![GitHub Workflow Status (with event)](https://img.shields.io/github/actions/workflow/status/thomhurst/ModularPipelines/dotnet.yml) ![GitHub last commit (branch)](https://img.shields.io/github/last-commit/thomhurst/ModularPipelines/main) [![Codacy Badge](https://app.codacy.com/project/badge/Grade/5f14420d97304b42a9e96861a4c0fec4)](https://app.codacy.com/gh/thomhurst/ModularPipelines/dashboard?utm_source=gh\&utm_medium=referral\&utm_content=\&utm_campaign=Badge_grade) [![CodeFactor](https://www.codefactor.io/repository/github/thomhurst/modularpipelines/badge)](https://www.codefactor.io/repository/github/thomhurst/modularpipelines) ![License](https://img.shields.io/github/license/thomhurst/ModularPipelines) [![Codacy Badge](https://app.codacy.com/project/badge/Coverage/5f14420d97304b42a9e96861a4c0fec4)](https://app.codacy.com/gh/thomhurst/ModularPipelines/dashboard?utm_source=gh\&utm_medium=referral\&utm_content=\&utm_campaign=Badge_coverage) [![codecov](https://codecov.io/gh/thomhurst/ModularPipelines/graph/badge.svg?token=QC48Q6JOM4)](https://codecov.io/gh/thomhurst/ModularPipelines)

## The Problem with YAML Pipelines

You know the drill. You write some YAML, push it, wait for CI to start, watch it fail on a typo, fix it, push again, wait again. Repeat until you lose the will to live.

YAML pipelines are:

* **Impossible to debug locally** - "Works on my machine" but fails mysteriously in CI
* **No compile-time safety** - Typos in variable names? Enjoy your 10-minute feedback loop
* **Copy-paste hell** - Reusing logic means duplicating YAML and hoping you update all the copies
* **Vendor lock-in** - Switching from GitHub Actions to Azure Pipelines? Rewrite everything

## The Solution

ModularPipelines lets you write your CI/CD pipelines as regular C# code. That means:

**Set a breakpoint. Step through your pipeline. Fix it before you push.**

```csharp
[DependsOn<BuildModule>]
[DependsOn<TestModule>]
public class PublishModule : Module<CommandResult>
{
    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // This is real C#. Set a breakpoint. Inspect variables. Debug locally.
        return await context.Tools.DotNet.PublishAsync(new DotNetPublishOptions
        {
            ProjectSolution = "src/MyApp/MyApp.csproj",
            Configuration = "Release",
            Output = "publish/"
        }, cancellationToken: cancellationToken);
    }
}
```

## Why Developers Choose ModularPipelines

### Your IDE Actually Helps You

Intellisense, refactoring, compile-time errors. Your pipeline code gets the same treatment as your application code. Rename a module? Your IDE updates all the references. Typo in an option? Red squiggle before you even save.

### Run Locally, Push Confidently

Test your entire pipeline on your machine before pushing. No more "let me push and see if it works" commits. Debug failures in your IDE instead of reading logs from a build agent.

### Automatic Parallelization

Modules declare their dependencies with attributes. ModularPipelines figures out what can run in parallel and maximizes throughput. No more manually orchestrating parallel jobs.

### Switch Build Systems Without Rewriting

Your pipeline logic lives in C#, not in vendor-specific YAML. Moving from GitHub Actions to Azure Pipelines to TeamCity? Change one line - your modules stay the same.

### Full Dependency Injection

Inject services, configuration, and secrets the same way you do in ASP.NET Core. Mock dependencies for testing. No more environment variable gymnastics.

### Secrets Stay Secret

Secrets are automatically obfuscated in logs. No more accidentally exposing API keys in build output.

### Modules Share Data

Modules return strongly-typed results that other modules can consume. No shared mutable state - just clean data flow.

```csharp
// BuildModule returns version info
public class BuildModule : Module<BuildInfo>
{
    protected override async Task<BuildInfo> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await context.Tools.DotNet.BuildAsync(
            new DotNetBuildOptions { ProjectSolution = "MyApp.csproj" },
            cancellationToken: cancellationToken);
        return new BuildInfo { Version = "1.0.0", OutputPath = "bin/Release" };
    }
}

// PublishModule retrieves and uses it
[DependsOn<BuildModule>]
public class PublishModule : Module
{
    protected override async Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var buildResult = await context.GetModule<BuildModule>();
        var outputPath = buildResult.Value.OutputPath; // Throws with module context if unavailable
        // Publish using the build output...
    }
}
```

### Catch Mistakes at Compile Time

Built-in Roslyn analyzers catch common mistakes before you even run:

* Missing `[DependsOn]` when calling `GetModule<T>()`
* Circular dependencies between modules
* Forgetting to `await` module results
* Using `Console.Write` instead of the logging system

## [Full Documentation](https://thomhurst.github.io/ModularPipelines)

## Quick Start

```bash
dotnet new install ModularPipelines.Templates
dotnet new modularpipeline -n MyPipeline \
  --solution ../MySolution.slnx \
  --publish-project ../src/MyApp/MyApp.csproj
cd MyPipeline
dotnet run
```

The generated project contains separate restore, build, test, and publish modules with
explicit dependencies and configurable paths. See the
[template source](src/ModularPipelines.Templates/templates/modularpipeline) for a
complete copy-ready example.

Adding pipeline modules to an existing project instead? Install the core framework and
the .NET CLI integration used by the examples above:

```bash
dotnet add package ModularPipelines
dotnet add package ModularPipelines.DotNet
```

Then configure and execute the pipeline from `Program.cs`:

```csharp
using ModularPipelines;

using var builder = Pipeline.CreateBuilder(args);
await builder.ExecutePipelineAsync();
```

## Console Progress

See exactly what's happening as your pipeline runs:

![image](https://github.com/thomhurst/ModularPipelines/assets/30480171/7d85af1e-abfd-40c4-8ef6-5df06baa88d6)

## Results

Get a clear summary when your pipeline completes:

<img width="444" alt="image" src="https://github.com/thomhurst/ModularPipelines/assets/30480171/8963e891-2c29-4382-9a3e-6ced4daf4d4b">

## Integrations

ModularPipelines has strongly-typed wrappers for the tools you already use:

%%% AVAILABLE MODULES PLACEHOLDER %%%

## How Does This Compare to Cake / Nuke?

| | ModularPipelines | Cake | Nuke |
|---|---|---|---|
| **Language** | Real C# | C# DSL (scripted) | Real C# |
| **Parallelization** | Automatic based on dependencies | Manual | Manual |
| **Architecture** | Separate module classes (SRP) | Single build script | Single build class |
| **Dependency Injection** | Full Microsoft.Extensions.DI | Limited | Built-in but different |
| **Setup** | `dotnet run` | Requires bootstrapper | Requires global tool |
| **Module Communication** | Strongly-typed return values | Shared state | Parameters |

ModularPipelines takes a different approach: each unit of work is a self-contained module class. This keeps code organized, makes merge conflicts rare, and lets you test modules in isolation.

## Features at a Glance

* **Parallel execution** - Automatic based on declared dependencies
* **Module data sharing** - Strongly-typed results flow between modules
* **Roslyn analyzers** - Catch mistakes at compile time, not runtime
* **Conditional dependencies** - `DependsOnIf<T>()` for dynamic dependency graphs
* **Dependency management** - Circular dependency detection built-in
* **Strong typing** - Pass data between modules with compile-time safety
* **Debug locally** - Set breakpoints, inspect variables, fix issues before pushing
* **Build agent agnostic** - Same code runs on GitHub, Azure, TeamCity, or your laptop
* **Secret obfuscation** - Automatic masking in logs
* **Hooks** - Run code before/after any module
* **Skip conditions** - Dynamically skip modules based on custom logic
* **Retry policies** - Configurable retry with Polly integration
* **Requirements validation** - Check prerequisites before running
* **Progress reporting** - Real-time console output with parallel execution visualization
* **Source controlled** - Your pipeline is code, version it like code

## Breaking Changes

While I aim to maintain stability, minor versions may include breaking changes. These will always be documented in release notes.
