---
title: Timeouts
---

# Timeouts

When creating modules, you can set a timeout per module by overriding the `Timeout` property. You can set this to any timespan you like. Just bear in mind some build runners, like GitHub actions, have their own timeouts, so extending past these won't help.

## Example

```csharp
public class MyModule : Module
{
    protected override TimeSpan Timeout { get; } = TimeSpan.FromSeconds(120);

    protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        // Do something
    }
}
```