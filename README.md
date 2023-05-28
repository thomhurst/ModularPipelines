# ModularPipelines

## Why ModularPipelines

- C# / .NET
    - There's less of a learning curve if you're not familiar with different pipeline systems, or scripting, etc. Having it defined in a familiar language makes things easier
    - It's flexible - .NET has lots of powerful features out of the box, and we can tap into the ecosystem of libraries if we want anything more
    - Strong typing
    - Tap into the familiar .NET Host framework, providing full Dependency Injection, flexible Configuration builders 

- Source Control 
    - Some Pipelines, such as TeamCity built ones, aren't source controlled. This means that making changes for a new feature that isn't released yet has to happen globally, which can cause build breakages between different branches. With it in Source Control, we can change the pipeline on a branch for a new feature without affecting other builds and branches
    - A broken pipeline shouldn't ever get merged into the main branch if it never went green
    - We are able to easily look back at a history of changes if they're all stored in git commits
    - We are easily able to identify who made what changes

- Running locally
    - You can debug and run a pipeline locally (targeting test environments ideally), if you ever encounter a broken pipeline
    - How many times have you started a new role or team, and been provided a huge developer setup guide that requires you to download and install numerous different things? You could define a pipeline for local machines. You can decide in startup, depending on something like the Environment name, and decide to either register modules for setting up a local developer machine, or if on a build agent, for deploying to the cloud.  

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
    }
}
```

## Dependencies
The default behaviour is for modules to run in parallel, to speed up a pipeline as much as possible.
If you don't want a particular module to start until another one has finished, then you simply add a `[DependsOn<TModule>]` attribute to your module class

```csharp
[DependsOn<Module1>]
public class Module2 : Module
{
    ...
}
```

## Example
The pipeline to test, generate and upload the NuGet packages for this library.. is made from this library. See the ModularPipelines.Build project in this repository.