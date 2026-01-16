# ModularPipelines

**Write CI/CD pipelines in C#. Debug them locally. Ship with confidence.**

[![nuget](https://img.shields.io/nuget/v/ModularPipelines.svg)](https://www.nuget.org/packages/ModularPipelines/)

![Nuget](https://img.shields.io/nuget/dt/ModularPipelines) ![GitHub Workflow Status (with event)](https://img.shields.io/github/actions/workflow/status/thomhurst/ModularPipelines/dotnet.yml) ![GitHub last commit (branch)](https://img.shields.io/github/last-commit/thomhurst/ModularPipelines/main) [![Codacy Badge](https://app.codacy.com/project/badge/Grade/5f14420d97304b42a9e96861a4c0fec4)](https://app.codacy.com/gh/thomhurst/ModularPipelines/dashboard?utm_source=gh\&utm_medium=referral\&utm_content=\&utm_campaign=Badge_grade) [![CodeFactor](https://www.codefactor.io/repository/github/thomhurst/modularpipelines/badge)](https://www.codefactor.io/repository/github/thomhurst/modularpipelines) ![License](https://img.shields.io/github/license/thomhurst/ModularPipelines) [![Codacy Badge](https://app.codacy.com/project/badge/Coverage/5f14420d97304b42a9e96861a4c0fec4)](https://app.codacy.com/gh/thomhurst/ModularPipelines/dashboard?utm_source=gh\&utm_medium=referral\&utm_content=\&utm_campaign=Badge_coverage) [![codecov](https://codecov.io/gh/thomhurst/ModularPipelines/graph/badge.svg?token=QC48Q6JOM4)](https://codecov.io/gh/thomhurst/ModularPipelines)

## The Problem with YAML Pipelines

You know the drill. You write some YAML, push it, wait for CI to start, watch it fail on a typo, fix it, push again, wait again. Repeat until you lose the will to live.

YAML pipelines are:
- **Impossible to debug locally** - "Works on my machine" but fails mysteriously in CI
- **No compile-time safety** - Typos in variable names? Enjoy your 10-minute feedback loop
- **Copy-paste hell** - Reusing logic means duplicating YAML and hoping you update all the copies
- **Vendor lock-in** - Switching from GitHub Actions to Azure Pipelines? Rewrite everything

## The Solution

ModularPipelines lets you write your CI/CD pipelines as regular C# code. That means:

**Set a breakpoint. Step through your pipeline. Fix it before you push.**

```csharp
[DependsOn<BuildModule>]
[DependsOn<TestModule>]
public class PublishModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        // This is real C#. Set a breakpoint. Inspect variables. Debug locally.
        return await context.DotNet().Publish(new DotNetPublishOptions
        {
            Project = "src/MyApp/MyApp.csproj",
            Configuration = Configuration.Release,
            Output = "publish/"
        }, cancellationToken);
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
    protected override async Task<BuildInfo?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await context.DotNet().Build(new DotNetBuildOptions { Project = "MyApp.csproj" }, cancellationToken);
        return new BuildInfo { Version = "1.0.0", OutputPath = "bin/Release" };
    }
}

// PublishModule retrieves and uses it
[DependsOn<BuildModule>]
public class PublishModule : Module
{
    protected override async Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var buildResult = await GetModule<BuildModule>();
        var outputPath = buildResult.Value!.OutputPath; // Strongly-typed, compile-time checked
        // Publish using the build output...
    }
}
```

### Catch Mistakes at Compile Time
Built-in Roslyn analyzers catch common mistakes before you even run:
- Missing `[DependsOn]` when calling `GetModule<T>()`
- Circular dependencies between modules
- Forgetting to `await` module results
- Using `Console.Write` instead of the logging system

## [Full Documentation](https://thomhurst.github.io/ModularPipelines)

## Quick Start

```bash
dotnet new console -n MyPipeline
cd MyPipeline
dotnet add package ModularPipelines
```

```csharp
// Program.cs
await PipelineHostBuilder.Create()
    .AddModule<BuildModule>()
    .AddModule<TestModule>()
    .AddModule<PublishModule>()
    .ExecutePipelineAsync();
```

```csharp
// BuildModule.cs
public class BuildModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        return await context.DotNet().Build(new DotNetBuildOptions
        {
            Project = "MySolution.sln",
            Configuration = Configuration.Release
        }, cancellationToken);
    }
}
```

```csharp
// TestModule.cs
[DependsOn<BuildModule>]
public class TestModule : Module<CommandResult>
{
    protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        return await context.DotNet().Test(new DotNetTestOptions
        {
            Project = "MySolution.sln",
            Configuration = Configuration.Release
        }, cancellationToken);
    }
}
```

Run it:
```bash
dotnet run
```

That's it. No YAML. No waiting for CI. Just `dotnet run` and watch your pipeline execute.

## Console Progress

See exactly what's happening as your pipeline runs:

![image](https://github.com/thomhurst/ModularPipelines/assets/30480171/7d85af1e-abfd-40c4-8ef6-5df06baa88d6)

## Results

Get a clear summary when your pipeline completes:

<img width="444" alt="image" src="https://github.com/thomhurst/ModularPipelines/assets/30480171/8963e891-2c29-4382-9a3e-6ced4daf4d4b">

## Integrations

ModularPipelines has strongly-typed wrappers for the tools you already use:

| Package | Description | Version |
| --- | --- | --- |
| ModularPipelines | Write your pipelines in C#! | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.svg)](https://www.nuget.org/packages/ModularPipelines/) |
| ModularPipelines.AmazonWebServices | Helpers for interacting with Amazon Web Services. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.AmazonWebServices.svg)](https://www.nuget.org/packages/ModularPipelines.AmazonWebServices/) |
| ModularPipelines.Azure | Helpers for interacting with Azure. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Azure.svg)](https://www.nuget.org/packages/ModularPipelines.Azure/) |
| ModularPipelines.Azure.Pipelines | Helpers for interacting with Azure Pipeline agents. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Azure.Pipelines.svg)](https://www.nuget.org/packages/ModularPipelines.Azure.Pipelines/) |
| ModularPipelines.Chocolatey | Helpers for interacting with the Chocolatey CLI. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Chocolatey.svg)](https://www.nuget.org/packages/ModularPipelines.Chocolatey/) |
| ModularPipelines.Cmd | Helpers for interacting with the Windows cmd process. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Cmd.svg)](https://www.nuget.org/packages/ModularPipelines.Cmd/) |
| ModularPipelines.Docker | Helpers for interacting with the Docker CLI. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Docker.svg)](https://www.nuget.org/packages/ModularPipelines.Docker/) |
| ModularPipelines.DotNet | Helpers for interacting with dotnet CLI. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.DotNet.svg)](https://www.nuget.org/packages/ModularPipelines.DotNet/) |
| ModularPipelines.Email | Helpers for sending emails. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Email.svg)](https://www.nuget.org/packages/ModularPipelines.Email/) |
| ModularPipelines.Ftp | Helpers for downloading and uploading via FTP. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Ftp.svg)](https://www.nuget.org/packages/ModularPipelines.Ftp/) |
| ModularPipelines.Git | Helpers for interacting with git. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Git.svg)](https://www.nuget.org/packages/ModularPipelines.Git/) |
| ModularPipelines.GitHub | Helpers for interacting with GitHub Actions build agents. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.GitHub.svg)](https://www.nuget.org/packages/ModularPipelines.GitHub/) |
| ModularPipelines.Google | Helpers for interacting with the Google gcloud CLI. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Google.svg)](https://www.nuget.org/packages/ModularPipelines.Google/) |
| ModularPipelines.Helm | Helpers for interacting with Helm CLI. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Helm.svg)](https://www.nuget.org/packages/ModularPipelines.Helm/) |
| ModularPipelines.Kubernetes | Helpers for interacting with kubectl CLI. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Kubernetes.svg)](https://www.nuget.org/packages/ModularPipelines.Kubernetes/) |
| ModularPipelines.MicrosoftTeams | Helpers for sending Microsoft Teams cards. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.MicrosoftTeams.svg)](https://www.nuget.org/packages/ModularPipelines.MicrosoftTeams/) |
| ModularPipelines.Node | Helpers for interacting with node / npm CLI. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Node.svg)](https://www.nuget.org/packages/ModularPipelines.Node/) |
| ModularPipelines.Slack | Helpers for sending Slack cards. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Slack.svg)](https://www.nuget.org/packages/ModularPipelines.Slack/) |
| ModularPipelines.TeamCity | Helpers for interacting with TeamCity build agents. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.TeamCity.svg)](https://www.nuget.org/packages/ModularPipelines.TeamCity/) |
| ModularPipelines.Terraform | Helpers for interacting with Terraform CLI. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Terraform.svg)](https://www.nuget.org/packages/ModularPipelines.Terraform/) |
| ModularPipelines.WinGet | Helpers for interacting with the Windows Package Manager. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.WinGet.svg)](https://www.nuget.org/packages/ModularPipelines.WinGet/) |
| ModularPipelines.Yarn | Helpers for interacting with Yarn CLI. | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Yarn.svg)](https://www.nuget.org/packages/ModularPipelines.Yarn/) |

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

- **Parallel execution** - Automatic based on declared dependencies
- **Module data sharing** - Strongly-typed results flow between modules
- **Roslyn analyzers** - Catch mistakes at compile time, not runtime
- **Conditional dependencies** - `DependsOnIf<T>()` for dynamic dependency graphs
- **Dependency management** - Circular dependency detection built-in
- **Strong typing** - Pass data between modules with compile-time safety
- **Debug locally** - Set breakpoints, inspect variables, fix issues before pushing
- **Build agent agnostic** - Same code runs on GitHub, Azure, TeamCity, or your laptop
- **Secret obfuscation** - Automatic masking in logs
- **Hooks** - Run code before/after any module
- **Skip conditions** - Dynamically skip modules based on custom logic
- **Retry policies** - Configurable retry with Polly integration
- **Requirements validation** - Check prerequisites before running
- **Progress reporting** - Real-time console output with parallel execution visualization
- **Source controlled** - Your pipeline is code, version it like code

## Breaking Changes

While I aim to maintain stability, minor versions may include breaking changes. These will always be documented in release notes.
