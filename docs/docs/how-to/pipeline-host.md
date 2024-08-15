---
title: Pipeline Host
sidebar_position: 1
---

# Pipeline Host

To begin creating your pipeline, your entry point is the `PipelineHostBuilder`. 

The recommended way is to create a console application and then using this Host builder in your `Program.cs` main method.

The `PipelineHostBuilder` is based off of the Microsoft Generic Host, and provides access to familiar functionality:
- Configuration Builders
- Dependency Injection

It also exposes methods specific for a Pipeline, such as registering your custom Modules, or plugging in custom services, such as time providers, or setting log levels.

Modules can be registered directly on the `PipelineHostBuilder`, or if you want to conditionally register them, they can also be registered on the `IServiceCollection` within the `ConfigureServices` method. This gives you access to the Host context, so you have the environment and configuration available to use in any logic.

Once your pipeline has been configured, simply call `ExecutePipelineAsync` and `await` it. 

An example pipeline can be seen here:

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

        if (context.HostingEnvironment.IsDevelopment()) 
        {
            collection.AddModule<Module1>()
                .AddModule<Module2>();
        }
        else 
        {
            collection.AddModule<Module3>()
                .AddModule<Module4>();
        }
    })
    .AddModule<Module5>()
    .AddModule<Module6>();
    .ExecutePipelineAsync();
```
