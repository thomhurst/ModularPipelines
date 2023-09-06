# ModularPipelines

Define your pipeline in .NET! Strong types, intellisense, parallelisation, and the entire .NET ecosystem at your fingertips.

[![nuget](https://img.shields.io/nuget/v/ModularPipelines.svg)](https://www.nuget.org/packages/ModularPipelines/)
![Nuget](https://img.shields.io/nuget/dt/ModularPipelines)
![GitHub Workflow Status (with event)](https://img.shields.io/github/actions/workflow/status/thomhurst/ModularPipelines/dotnet.yml)
![GitHub last commit (branch)](https://img.shields.io/github/last-commit/thomhurst/ModularPipelines/main)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/5f14420d97304b42a9e96861a4c0fec4)](https://app.codacy.com/gh/thomhurst/ModularPipelines/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
![License](https://img.shields.io/github/license/thomhurst/ModularPipelines)

## Available Modules

%%% AVAILABLE MODULES PLACEHOLDER %%%

## Getting Started

If you want to see how to get started, or want to know more about ModularPipelines, [read the Wiki page here](https://github.com/thomhurst/ModularPipelines/wiki)

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
    })
    .AddModule<FindNugetPackagesModule>()
    .AddModule<UploadNugetPackagesModule>()
    .ExecutePipelineAsync();
```

### Custom Modules

```csharp
public class FindNugetPackagesModule : Module<FileInfo>
{
    protected override async Task<List<File>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Yield();

        return context.FileSystem.GetFiles(context.Environment.GitRootDirectory!.Path,
            SearchOption.AllDirectories,
            path => path.Extension is ".nupkg")
            .ToList();
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

    protected override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var nugetFiles = await GetModule<FindNugetPackagesModule>();

        return await context.NuGet()
            .UploadPackages(new NuGetUploadOptions(packagePaths.Value!.AsPaths(), new Uri("https://api.nuget.org/v3/index.json"))
            {
                ApiKey = _nugetSettings.Value.ApiKey,
                NoSymbols = true
            });
    }
}
```
