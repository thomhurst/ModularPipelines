---
title: Requirements
---

# Requirements

If you'd like to fail fast, you can register some `Requirement` classes that do some checks on start up to make sure things are as expected. 

Simply implement `IPipelineRequirement` and then call `IServiceCollection.AddRequirement<TRequirement>()`

```csharp
public class WindowsRequirement : IPipelineRequirement
{
    public Task<bool> MustAsync(IPipelineHookContext context)
    {
        return (context.Environment.OperatingSystem == OSPlatform.Windows).AsTask();
    }
}
```