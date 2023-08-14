# ModularPipelines

[![nuget](https://img.shields.io/nuget/v/ModularPipelines.svg)](https://www.nuget.org/packages/ModularPipelines/)
![Nuget](https://img.shields.io/nuget/dt/ModularPipelines)
![GitHub Workflow Status (with event)](https://img.shields.io/github/actions/workflow/status/thomhurst/ModularPipelines/dotnet.yml)
![GitHub last commit (branch)](https://img.shields.io/github/last-commit/thomhurst/ModularPipelines/main)


## Work in Progress
I'd really appreciate some feedback around what people think, and anything that could improved.
If you'd like to install, the packages are available on NuGet, just make sure to show 'prerelease' packages!

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
| ModularPipelines.Terraform | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Terraform.svg)](https://www.nuget.org/packages/ModularPipelines.Terraform/) |


## Why ModularPipelines

- C# / .NET

  - Familiar - If you're a .NET developer, you don't have to concern yourself as much with the different features, languages or syntaxes of different build systems
  - It's flexible - .NET has lots of powerful features out of the box, and we can tap into the ecosystem of libraries if we want anything more
  - Strong typing - We can structure our modules with objects that we can pass around, and we know what data we then have available to use
  - Tap into the familiar .NET Host framework, providing a familiar Startup, full Dependency Injection, flexible Configuration builders and more

- Source Control

  - Some Pipelines, such as TeamCity, aren't source controlled. This means that making changes for a new feature that isn't released yet has to happen globally, which can cause build breakages between different branches. With it in Source Control, we can change the pipeline on a branch for a new feature without affecting other builds and branches
  - A broken pipeline shouldn't ever get merged into the main branch if it never went green
  - We are able to easily look back at a history of changes if they're all stored in git commits
  - We are easily able to identify who made what changes

- Running locally

  - You can debug and run a pipeline locally (targeting test environments ideally), if you ever encounter a broken pipeline
  - How many times have you started a new role or team, and been provided a huge developer setup guide that requires you to download and install numerous different things? You could define a pipeline for local machines. You can decide in startup, depending on something like the Environment name, and decide to either register modules for setting up a local developer machine, or if on a build agent, for deploying to the cloud.

- Portability

  - Want to move to a different build system? You don't have to re-learn or setup the whole thing from scratch. Your system simply needs access to your Pipeline project and have the .NET SDK installed.

## Getting Started

If you want to see how to get started, [read the Wiki page here](https://github.com/thomhurst/ModularPipelines/wiki)

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

        collection.AddModule<Module1>()
            .AddModule<Module2>()
            .AddModule<Module3>()
            .AddModule<Module4>();
    })
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
    protected override async Task<ModuleResult<CommandResult>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var nugetFiles = await GetModule<FindNugetPackagesModule>();

        return await context.NuGet()
            .UploadPackages(new NuGetUploadOptions(packagePaths.Value!.AsPaths(), new Uri("https://api.nuget.org/v3/index.json"))
            {
                ApiKey = "SomeApiKey",
                NoSymbols = true
            });
    }
}
```
