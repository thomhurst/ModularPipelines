# ModularPipelines

Define your pipeline in .NET! Strong types, intellisense, parallelisation, and the entire .NET ecosystem at your fingertips.

[![nuget](https://img.shields.io/nuget/v/ModularPipelines.svg)](https://www.nuget.org/packages/ModularPipelines/)

![Nuget](https://img.shields.io/nuget/dt/ModularPipelines) ![GitHub Workflow Status (with event)](https://img.shields.io/github/actions/workflow/status/thomhurst/ModularPipelines/dotnet.yml) ![GitHub last commit (branch)](https://img.shields.io/github/last-commit/thomhurst/ModularPipelines/main) [![Codacy Badge](https://app.codacy.com/project/badge/Grade/5f14420d97304b42a9e96861a4c0fec4)](https://app.codacy.com/gh/thomhurst/ModularPipelines/dashboard?utm_source=gh\&utm_medium=referral\&utm_content=\&utm_campaign=Badge_grade) [![CodeFactor](https://www.codefactor.io/repository/github/thomhurst/modularpipelines/badge)](https://www.codefactor.io/repository/github/thomhurst/modularpipelines) ![License](https://img.shields.io/github/license/thomhurst/ModularPipelines) [![Codacy Badge](https://app.codacy.com/project/badge/Coverage/5f14420d97304b42a9e96861a4c0fec4)](https://app.codacy.com/gh/thomhurst/ModularPipelines/dashboard?utm_source=gh\&utm_medium=referral\&utm_content=\&utm_campaign=Badge_coverage) [![codecov](https://codecov.io/gh/thomhurst/ModularPipelines/graph/badge.svg?token=QC48Q6JOM4)](https://codecov.io/gh/thomhurst/ModularPipelines)

## [Documentation](https://thomhurst.github.io/ModularPipelines)

<https://thomhurst.github.io/ModularPipelines>

## Features

*   Parallel execution
*   Dependency management
*   Familiar C# code
*   Ability to debug pipelines
*   Ability to run pipelines locally, even creating versions for setting up local development
*   Strong typing, where different modules/steps can pass data to one another
*   Dependency collision detection - Don't worry about accidentally making two modules dependent on each other
*   Numerous helpers to do things like: Search files, check checksums, (un)zip folders, download files, install files, execute CLI commands, hash data, and more
*   Easy to Skip or Ignore Failures for each individual module by passing in custom logic
*   Hooks that can run before and/or after modules
*   Pipeline requirements - Validate your requirements are met before executing your pipeline, such as a Linux operating system
*   Easy to use File and Folder classes, that can search, read, update, delete and more
*   Source controlled pipelines
*   Build agent agnostic - Can easily move to a different build system without completely recreating your pipeline
*   No need to learn new syntaxes such as YAML defined pipelines
*   Strongly typed wrappers around command line tools
*   Utilise existing .NET libraries
*   Secret obfuscation
*   Grouped logging, and the ability to extend sources by adding to the familiar `ILogger`
*   Run based on categories
*   Easy to read exceptions
*   Dynamic console progress reporting (if the console supports interactive mode)
*   Pretty results table

## Available Modules

%%% AVAILABLE MODULES PLACEHOLDER %%%

## Getting Started

If you want to see how to get started, or want to know more about ModularPipelines, [read the Documentation here](https://thomhurst.github.io/ModularPipelines)

## Console Progress

![image](https://github.com/thomhurst/ModularPipelines/assets/30480171/7d85af1e-abfd-40c4-8ef6-5df06baa88d6)

## Results

<img width="444" alt="image" src="https://github.com/thomhurst/ModularPipelines/assets/30480171/8963e891-2c29-4382-9a3e-6ced4daf4d4b">

## How does this compare to Cake / Nuke

*   Strong types! You have complete control over what data, and what shape of data to pass around from and to different modules
*   No external tooling is required. Pipelines are run with a simple `dotnet run`
*   Full dependency injection support for your services
*   Similar and familiar setup to frameworks like ASP.NET Core
*   Real C# - Whereas frameworks like cake are a scripted form of C#
*   Parallelism - Work will run concurrently unless it is dependent on something else
*   The style of writing pipelines is very different - Work is organised into separate module classes, keeping code organised and more closely following SRP than having all your work in one main class. This also helps multiple contributors avoid things like merge conflicts

## Code Examples

### Program.cs - Main method

```csharp
await PipelineHostBuilder.Create()
    .ConfigureAppConfiguration((context, builder) =>
    {
        builder.AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables();
    })
    .ConfigureServices((context, collection) =>
    {
        collection.Configure<NuGetSettings>(context.Configuration.GetSection("NuGet"));
        collection.Configure<PublishSettings>(context.Configuration.GetSection("Publish"));
        collection.AddSingleton<ISomeService1, SomeService1>();
        collection.AddTransient<ISomeService2, SomeService2>();
    })
    .AddModule<FindNugetPackagesModule>()
    .AddModule<UploadNugetPackagesModule>()
    .ExecutePipelineAsync();
```

### Custom Modules

```csharp
public class FindNugetPackagesModule : Module<FileInfo>
{
    protected override Task<List<File>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        return context.Git()
            .RootDirectory
            .GetFiles(path => path.Extension is ".nupkg")
            .ToList()
            .AsTask();
    }
}
```

```csharp
[DependsOn<FindNugetPackagesModule>]
public class UploadNugetPackagesModule : Module<FileInfo>
{
    private readonly IOptions<NuGetSettings> _nugetSettings;

    public UploadNugetPackagesModule(IOptions<NuGetSettings> nugetSettings)
    {
        _nugetSettings = nugetSettings;
    }

    protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var nugetFiles = await GetModule<FindNugetPackagesModule>();

        return await nugetFiles.Value!
            .SelectAsync(async nugetFile => await context.DotNet().Nuget.Push(new DotNetNugetPushOptions
            {
                Path = nugetFile,
                Source = "https://api.nuget.org/v3/index.json",
                ApiKey = _nugetSettings.Value.ApiKey!,
            }, cancellationToken), cancellationToken: cancellationToken)
            .ProcessOneAtATime();
    }
}
```

### Breaking changes

While I will try to limit breaking changes, there may be some changes within minor versions. These will be noted on release notes.
