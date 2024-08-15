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

You can also override things such as Timeouts, OnBefore and OnAfter methods, on a module by module basis.
