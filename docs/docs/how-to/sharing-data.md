---
title: Sharing data across modules
sidebar_position: 4
---

# Sharing data across modules

Modules have been designed with data and sharing at its core.

When a module returns data in its `ExecuteAsync` method, that data is available to be seen by other modules.

Simply call `await GetModule<TModule>()` from within your module, and you'll have access to that data.

```csharp
var myModuleResultObject = await GetModule<MyModule>();

await DoSomething(myModuleResultObject.Value);
```

We can also detect if the module failed or was skipped:

```csharp
        if (myModuleResultObject.ModuleResultType == ModuleResultType.Failure)
        {
            // Do something
        } 
        else if (myModuleResultObject.ModuleResultType == ModuleResultType.Skipped)
        {
            // Do something else
        } 
        else
        {
            // Do happy path
        }
```