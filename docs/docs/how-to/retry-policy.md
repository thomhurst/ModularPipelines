---
title: Retry Policies
sidebar_position: 6
---

# Retry Policies

When creating modules, you can set a retry policy per module by overriding the `RetryPolicy` property. The retry policy uses a Polly policy, so if you've used Polly before you should be familiar with how to use it.
If not, Polly have their own documentation that should help you with this.

## Default
Retry policies are off by default, unless you set a default retry count on the `PipelineOptions` when you're using the `PipelineHostBuilder`. If you set a retry count, for example, 3, then all modules that fail will attempt to retry 3 times. This can be useful for when transient failures occur. You can of course combine this with overriding specific modules for more control.

## Example

```csharp
public class MyModule : Module
{
    protected override AsyncRetryPolicy<IDictionary<string, object>?> RetryPolicy { get; }
        = Policy<IDictionary<string, object>?>
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(5, i => TimeSpan.FromSeconds(i * i));

    protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        // Do something
    }
}
```

## Default Example

```csharp
await PipelineHostBuilder.Create()
    .AddModule<Module1>()
    .AddModule<Module2>()
    .AddModule<Module3>()
    .ConfigurePipelineOptions((context, options) =>
    {
        options.DefaultRetryCount = 3;
    })
    .ExecutePipelineAsync();

```