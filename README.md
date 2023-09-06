# ModularPipelines

Define your pipeline in .NET! Strong types, intellisense, parallelisation, and the entire .NET ecosystem at your fingertips.

[![nuget](https://img.shields.io/nuget/v/ModularPipelines.svg)](https://www.nuget.org/packages/ModularPipelines/)
![Nuget](https://img.shields.io/nuget/dt/ModularPipelines)
![GitHub Workflow Status (with event)](https://img.shields.io/github/actions/workflow/status/thomhurst/ModularPipelines/dotnet.yml)
![GitHub last commit (branch)](https://img.shields.io/github/last-commit/thomhurst/ModularPipelines/main)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/5f14420d97304b42a9e96861a4c0fec4)](https://app.codacy.com/gh/thomhurst/ModularPipelines/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
![License](https://img.shields.io/github/license/thomhurst/ModularPipelines)

## Available Modules

| Package | Version |
| --- | --- |
| ModularPipelines | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.svg)](https://www.nuget.org/packages/ModularPipelines/) |
| ModularPipelines.Azure | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Azure.svg)](https://www.nuget.org/packages/ModularPipelines.Azure/) |
| ModularPipelines.Azure.Pipelines | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Azure.Pipelines.svg)](https://www.nuget.org/packages/ModularPipelines.Azure.Pipelines/) |
| ModularPipelines.Cmd | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Cmd.svg)](https://www.nuget.org/packages/ModularPipelines.Cmd/) |
| ModularPipelines.Docker | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Docker.svg)](https://www.nuget.org/packages/ModularPipelines.Docker/) |
| ModularPipelines.DotNet | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.DotNet.svg)](https://www.nuget.org/packages/ModularPipelines.DotNet/) |
| ModularPipelines.Email | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Email.svg)](https://www.nuget.org/packages/ModularPipelines.Email/) |
| ModularPipelines.Ftp | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Ftp.svg)](https://www.nuget.org/packages/ModularPipelines.Ftp/) |
| ModularPipelines.Git | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Git.svg)](https://www.nuget.org/packages/ModularPipelines.Git/) |
| ModularPipelines.Helm | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Helm.svg)](https://www.nuget.org/packages/ModularPipelines.Helm/) |
| ModularPipelines.Kubernetes | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Kubernetes.svg)](https://www.nuget.org/packages/ModularPipelines.Kubernetes/) |
| ModularPipelines.MicrosoftTeams | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.MicrosoftTeams.svg)](https://www.nuget.org/packages/ModularPipelines.MicrosoftTeams/) |
| ModularPipelines.Node | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Node.svg)](https://www.nuget.org/packages/ModularPipelines.Node/) |
| ModularPipelines.NuGet | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.NuGet.svg)](https://www.nuget.org/packages/ModularPipelines.NuGet/) |
| ModularPipelines.Slack | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Slack.svg)](https://www.nuget.org/packages/ModularPipelines.Slack/) |
| ModularPipelines.Terraform | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Terraform.svg)](https://www.nuget.org/packages/ModularPipelines.Terraform/) |


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
    protected override async Task<ModuleResult<List<File>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    protected override async Task<ModuleResult<CommandResult>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
