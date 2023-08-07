# ModularPipelines

[![nuget](https://img.shields.io/nuget/v/ModularPipelines.svg)](https://www.nuget.org/packages/ModularPipelines/)
![Nuget](https://img.shields.io/nuget/dt/ModularPipelines)
![GitHub Workflow Status (with event)](https://img.shields.io/github/actions/workflow/status/thomhurst/ModularPipelines/dotnet.yml)
![GitHub last commit (branch)](https://img.shields.io/github/last-commit/thomhurst/ModularPipelines/main)


## Work in Progress
I'd really appreciate some feedback around what people think, and anything that could improved.
If you'd like to install, the packages are available on NuGet, just make sure to show 'prerelease' packages!

## Available Modules

| Package | Version | URL |
| --- | --- | --- |
| ModularPipelines | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.svg)](https://www.nuget.org/packages/ModularPipelines/) | [https://nuget.org/packages/ModularPipelines](https://nuget.org/packages/ModularPipelines) |
| ModularPipelines.Azure | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Azure.svg)](https://www.nuget.org/packages/ModularPipelines.Azure/) | [https://nuget.org/packages/ModularPipelines.Azure](https://nuget.org/packages/ModularPipelines.Azure) |
| ModularPipelines.Azure.Pipelines | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Azure.Pipelines.svg)](https://www.nuget.org/packages/ModularPipelines.Azure.Pipelines/) | [https://nuget.org/packages/ModularPipelines.Azure.Pipelines](https://nuget.org/packages/ModularPipelines.Azure.Pipelines) |
| ModularPipelines.Cmd | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Cmd.svg)](https://www.nuget.org/packages/ModularPipelines.Cmd/) | [https://nuget.org/packages/ModularPipelines.Cmd](https://nuget.org/packages/ModularPipelines.Cmd) |
| ModularPipelines.Docker | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Docker.svg)](https://www.nuget.org/packages/ModularPipelines.Docker/) | [https://nuget.org/packages/ModularPipelines.Docker](https://nuget.org/packages/ModularPipelines.Docker) |
| ModularPipelines.DotNet | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.DotNet.svg)](https://www.nuget.org/packages/ModularPipelines.DotNet/) | [https://nuget.org/packages/ModularPipelines.DotNet](https://nuget.org/packages/ModularPipelines.DotNet) |
| ModularPipelines.Git | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Git.svg)](https://www.nuget.org/packages/ModularPipelines.Git/) | [https://nuget.org/packages/ModularPipelines.Git](https://nuget.org/packages/ModularPipelines.Git) |
| ModularPipelines.Helm | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Helm.svg)](https://www.nuget.org/packages/ModularPipelines.Helm/) | [https://nuget.org/packages/ModularPipelines.Helm](https://nuget.org/packages/ModularPipelines.Helm) |
| ModularPipelines.Kubernetes | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Kubernetes.svg)](https://www.nuget.org/packages/ModularPipelines.Kubernetes/) | [https://nuget.org/packages/ModularPipelines.Kubernetes](https://nuget.org/packages/ModularPipelines.Kubernetes) |
| ModularPipelines.MicrosoftTeams | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.MicrosoftTeams.svg)](https://www.nuget.org/packages/ModularPipelines.MicrosoftTeams/) | [https://nuget.org/packages/ModularPipelines.MicrosoftTeams](https://nuget.org/packages/ModularPipelines.MicrosoftTeams) |
| ModularPipelines.Node | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Node.svg)](https://www.nuget.org/packages/ModularPipelines.Node/) | [https://nuget.org/packages/ModularPipelines.Node](https://nuget.org/packages/ModularPipelines.Node) |
| ModularPipelines.NuGet | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.NuGet.svg)](https://www.nuget.org/packages/ModularPipelines.NuGet/) | [https://nuget.org/packages/ModularPipelines.NuGet](https://nuget.org/packages/ModularPipelines.NuGet) |
| ModularPipelines.Terraform | [![nuget](https://img.shields.io/nuget/v/ModularPipelines.Terraform.svg)](https://www.nuget.org/packages/ModularPipelines.Terraform/) | [https://nuget.org/packages/ModularPipelines.Terraform](https://nuget.org/packages/ModularPipelines.Terraform) |


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

## Registering Modules in Startup

ModularPipelines uses the familiar Host builder syntax, which you'll recognise if you've worked on .NET Web APIs.

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

## Contexts
There are a few packages for providing out-of-the-box functonality for things like DotNet commands, or publishing a card to Microsoft teams.
If you install any of these projects, then in startup, make sure you register these contexts:
```csharp
collection.RegisterDotNetContext();
```

These can then be used within your modules, by accessing them on the `IModuleContext` object
```csharp
await Context.DotNet().Build(new DotNetOptions
            {
                TargetPath = SomePath
            }, cancellationToken)
```

## Defining Modules

Modules are defined by creating a class that inherits from the `Module<T>` base class - And T is a return type, if you want your module to be able to return data, that you can retrieve from other modules. You can also just inherit from `Module` which will assume you're returning a dictionary of data. You can also return Nothing, which will be explained further down.

```csharp
public class FindAFileModule : Module<FileInfo>
{
    public FindAFileModule(IModuleContext context) : base(context)
    {
    }

    protected override async Task<ModuleResult<FileInfo>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();
        return Context.FileSystem
            .GetFiles("C:\\", SearchOption.AllDirectories, file => file.Name == "MyJsonFile.json")
            .Single();

        // or
        return await NothingAsync();
    }
}
```

You can also override things such as Timeouts, OnBefore and OnAfter methods, on a module by module basis.

## Execution and Dependencies

The default behaviour is for modules to run in parallel, to speed up a pipeline as much as possible. If you don't want a particular module to start until another one has finished, then you simply add a `[DependsOn<TModule>]` attribute to your module class

```csharp
[DependsOn<Module1>] // or [DependsOn(typeof(Module1))] for older language versions
public class Module2 : Module
{
    ...
}
```

## Sharing Data across modules

One module is able to see the data that another module has returned. Simply call `await GetModule<TModule>()` from within your module, and you'll have access to the object that it returned in its `ExecuteAsync` method.

```csharp
var myModuleResultObject = await GetModule<MyModule>();

await DoSomething(myModuleResultObject.Value);
```

## Hooks

As mentioned, we can define OnBefore and OnAfter behaviour in specific modules by overriding those methods. But if we want to have repeat behaviour for every module, we can register some 'Hook' classes during startup.

Pipeline Global Hooks will run once, before any modules have started, and/or after all modules have finished. Pipeline Module Hooks will run repeatedly, before every module, and/or after every module.

This can be useful if you want some standard logging behaviour for example.

```csharp
collection.AddPipelineGlobalHooks<MyGlobalHooks>()
            .AddPipelineModuleHooks<MyModuleHooks>()
```

```csharp
public class MyModuleHooks : IPipelineModuleHooks
{
    public Task OnBeforeModuleStartAsync(IModuleContext moduleContext, IModule module)
    {
        moduleContext.Logger.LogInformation("{Module} is starting", module.GetType().Name);
        return Task.CompletedTask;
    }

    public Task OnBeforeModuleEndAsync(IModuleContext moduleContext, IModule module)
    {
        moduleContext.Logger.LogInformation("{Module} finished after {Elapsed}", module.GetType().Name, module.Duration);
        return Task.CompletedTask;
    }
}
```

## Requirements

If you'd like to fail fast, you can register some `Requirement` classes that do some checks on start up to make sure things are as expected. Simply implement `IPipelineRequirement` and then call `IServiceCollection.AddRequirement<TRequirement>()`

```csharp
public class WindowsRequirement : IPipelineRequirement
{
    public async Task<bool> MustAsync()
    {
        await Task.Yield();
        return Environment.OSVersion.Platform == PlatformID.Win32NT;
    }
}
```

## Inheriting

Each 'Module' is expected to be registered only once. If you build a custom module that you'd like to instantiate multiple times but with different options, then you should create a new Module type that inherits from an abstract base module.

## Example

The pipeline to test, generate and upload the NuGet packages for this library.. is made from this library. See the ModularPipelines.Build project in this repository.
