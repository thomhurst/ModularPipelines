---
title: Defining Modules
sidebar_position: 2
---

## Defining Modules

Modules are defined by creating a class that inherits from the `Module<T>` base class.

`T` is the type of object that your Module will return, and that object can be seen by other Modules (if they depend on it.)
You can also just inherit from `Module` which will assume you're returning a dictionary of data. You can also return Nothing, which will be explained further down.

```csharp
public class FindAFileModule : Module<FileInfo>
{
    protected async Task<FileInfo?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        return context.FileSystem
            .GetFiles("C:\\", SearchOption.AllDirectories, file => file.Name == "MyJsonFile.json")
            .Single()
            .AsTask();

        // or
        return NothingAsync();
    }
}
```

You can also configure module behaviors such as timeouts, retry policies, skip conditions, and hooks by overriding the `Configure()` method:

```csharp
public class MyModule : Module<FileInfo>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithTimeout(TimeSpan.FromMinutes(5))
        .WithRetryCount(3)
        .WithSkipWhen(() => !File.Exists("important.json"))
        .Build();

    protected override async Task<FileInfo?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        // Module logic here
    }
}
```

See the individual documentation pages for more details on each behavior:
- [Skipping Modules](skipping)
- [Retry Policies](retry-policy)
- [Timeouts](timeouts)
- [Ignoring Failures](ignoring-failures)
- [Always Run](always-run)
- [Hooks](hooks)
