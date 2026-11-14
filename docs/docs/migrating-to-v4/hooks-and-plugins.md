---
title: Hooks and plugins
sidebar_position: 5
---

# Hooks, event handlers, and plugins

A module's own `OnBeforeExecuteAsync` / `OnAfterExecuteAsync` overrides are covered in
[Modules](./modules.md#lifecycle-hooks). This page covers handlers that observe other modules or
the whole pipeline.

## Event handlers

The interfaces are renamed and live in `ModularPipelines.Events`. Attribute handlers and global
handlers share them.

| V3 contract | V4 contract |
| --- | --- |
| `IPipelineGlobalHooks` | `IPipelineEventHandler` |
| `IPipelineModuleHooks` | `IModuleEventHandler` |
| `IPipelineHookContext` | `IPipelineContext` |
| `IEventHandlerPriority.Priority` | `IEventHandler.Order` (ascending, default `0`); global and registration handlers also gain `ContinueOnError` |
| `IModuleRegistrationEventReceiver` | `IModuleRegistrationHandler` |
| `IPipelineModuleHooks.OnModuleEndAsync(context)` | `OnModuleEndAsync(context, IModuleResult result, cancellationToken)` |
| `IPipelineModuleHooks.OnModuleFailureAsync(context)` | `OnModuleFailureAsync(context, Exception exception, cancellationToken)` |
| `IPipelineModuleHooks.OnModuleSkippedAsync(context)` | `OnModuleSkippedAsync(context, SkipDecision reason, cancellationToken)` |
| `IModuleHookContext.RequestRetry`, `SkipDependentModules`, `FailPipeline` | Removed; they had no effect. Use `WithRetry`, dependency declarations, or throw from the handler |
| `IModuleRegistrationContext.Services` | Removed; register services on `builder.Services` |

To migrate a handler:

1. Rename the interface.
2. Add a trailing `CancellationToken` to **every** method. This applies to module, pipeline, and
   registration handlers, for example
   `OnPipelineStartAsync(IPipelineContext context, CancellationToken cancellationToken)` and
   `OnRegistrationAsync(IModuleRegistrationContext context, CancellationToken cancellationToken)`.
3. Register it with `builder.AddPipelineEventHandler<T>()` or `builder.AddModuleEventHandler<T>()`.

Hand-written implementations or test fakes also need new members: `ModuleId` on
`IModuleHookContext`, and `RemoveDependency(Type)` on `IModuleRegistrationContext`.

See [Hooks](../how-to/hooks.md) for signatures and ordering.

### When a handler throws

Unless `ContinueOnError` is `true`:

| Handler | What a failure does |
| --- | --- |
| Ready, Start, registration, or pipeline handler | Fails the module or pipeline |
| End, Failure, or Skipped handler | Does not change the module outcome it observed. It is reported as an additional pipeline error |
| Pipeline-end handler | Never hides an earlier execution failure |

## Plugins

`IModularPipelinesPlugin` keeps `Name`. One `Configure(PipelineBuilder builder)` method replaces
`Priority`, `ConfigureServices`, and `ConfigurePipeline`. Plugins are registered on the builder, not
in a static registry.

```csharp
// V3
PluginRegistry.Register(new MyPlugin());

// V4
builder.AddPlugin<MyPlugin>(); // or builder.AddPlugin(new MyPlugin())

public sealed class MyPlugin : IModularPipelinesPlugin
{
    public string Name => "My plugin";

    public void Configure(PipelineBuilder builder)
    {
        builder.Services.AddSingleton<IMyService, MyService>();
        builder.AddModule<MyModule>();
    }
}
```

- `PluginRegistry` and `PluginTestHelper` are removed. Each builder owns its plugins, so remove
  `PluginTestHelper.IsolatedRegistry()` from tests.
- Plugins apply immediately, in the order you add them. Order your `AddPlugin` calls where the V3
  `Priority` mattered.
- Builder configuration after the `AddPlugin` call overrides the plugin's configuration.
- Adding two plugins with the same `Name`, or a blank `Name`, throws.
- A plugin that throws from `Configure` fails at `AddPlugin` with `PluginInitializationException`.
- Plugins that registered themselves from a module initializer must now be added explicitly by the
  consumer.
- Plugin assemblies that declare `[assembly: ModularPipelinesPlugin(3)]` must change it to `4`. A
  mismatched major version throws `PluginVersionMismatchException`.
