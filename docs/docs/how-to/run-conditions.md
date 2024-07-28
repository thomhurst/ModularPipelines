---
title: Run conditions
---

# Run conditions
Run conditions gives us a way to easily skip or run modules based on some custom, reusable logic.

Run conditions are generally controlled by attributes, placed upon your module.

Some of these exist out-of-the-box
e.g.
[RunOnLinux]
[RunOnWindows]
[RunOnMacOS]

You can create your own custom run conditions by inheriting from `RunConditionAttribute` and plugging custom logic into the `Condition` method.

```csharp
public class MyCustomRunConditionAttribute : RunConditionAttribute
{
    public override async Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        var response = await pipelineContext.Http.SendAsync("https://www.example.com/ping");
        return response.StatusCode == HttpStatusCode.OK;
    }
}
```

Returning `true` means the module will run. Returning `false` means it won't, UNLESS another run condition attribute returns true.

Multiple can be supplied, and only one needs to return `true`.

e.g.
```csharp
[RunOnLinux]
[RunOnMacOS]
public class MyModule : Module
```

The above module will run on either Linux, or Mac. But not windows.

## Mandatory run conditions

Mandatory run conditions are similar to standard run conditions, but they MUST return true to run the module. If ANY return false, then the module will not run.

You can create your own custom mandatory run conditions by inheriting from `MandatoryRunConditionAttribute` and plugging custom logic into the `Condition` method.

```csharp
public class MyMandatoryCustomRunConditionAttribute : MandatoryRunConditionAttribute
{
    public override async Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        var response = await pipelineContext.Http.SendAsync("https://www.example.com/service1/mustbeup/ping");
        return response.StatusCode == HttpStatusCode.OK;
    }
}

public class MyMandatoryCustomRunCondition2Attribute : MandatoryRunConditionAttribute
{
    public override async Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        var response = await pipelineContext.Http.SendAsync("https://www.example.com/service2/mustbeup/ping");
        return response.StatusCode == HttpStatusCode.OK;
    }
}
```

Take the above 2 mandatory conditions and this module:

```csharp
[MyMandatoryCustomRunCondition]
[MyMandatoryCustomRunCondition2]
public class MyModule : Module
```

This module will only run if BOTH conditions return true. If either return false, then the module will be skipped.