---
title: Modules
sidebar_position: 3
---

# Modules

## The `Configure` override

`Configure()` no longer returns a `ModuleConfiguration`. V4 hands you a builder in a `void`
override. There is no `ModuleConfiguration.Create()`, `.Default`, or final `.Build()`.

Dependencies and metadata move into the same method.

```csharp
// V3
protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
    .WithTimeout(TimeSpan.FromMinutes(10))
    .WithRetryCount(3)
    .Build();

protected override void DeclareDependencies(IDependencyDeclaration dependencies)
{
    dependencies.DependsOn<RestoreModule>();
}

public override string? Category => "Build";
```

```csharp
// V4
protected override void Configure(ModuleConfigurationBuilder module)
{
    base.Configure(module);
    module
        .DependsOn<RestoreModule>()
        .WithCategory("Build")
        .WithTimeout(TimeSpan.FromMinutes(10))
        .WithRetry(3);
}
```

- Call `base.Configure(module)` when you inherit configuration from another module class.
- Keep configuration deterministic. It is initialized once for execution and also used for
  planning. Use the context-aware callbacks for decisions that need runtime services.

| V3 pattern | V4 replacement |
| --- | --- |
| `DeclareDependencies(IDependencyDeclaration)` | `DependsOn`, `DependsOnOptional`, `DependsOnIf` on the supplied module builder |
| `DependsOnLazy<T>()` / `DependsOnLazy(Type)` | `DependsOnOptional<T>()` / `DependsOnOptional(Type)`; V3 lazy dependencies were already optional and did not defer execution |
| `DependsOnIf<T>(Func<bool>)` / `DependsOnIf(Type, Func<bool>)` | `DependsOnIf<T>(bool)` / `DependsOnIf(Type, bool)`; evaluate the predicate while configuring |
| `Tags` / `Category` overrides | `WithTags(...)` / `WithCategory(...)`, or the existing attributes |
| `WithRetryCount(count)` | `WithRetry(count, baseDelay: ..., shouldRetry: ...)` |
| `WithRetryPolicy(Polly.IAsyncPolicy)` | `WithRetry(...)` or `WithShield(Kevlar.Shield)` |
| `WithRetryPolicy(Func<IModuleContext, IAsyncPolicy>)` | `WithShield(Func<IModuleContext, Shield>)` |
| `WithBeforeExecute(...)` / `WithAfterExecute(...)` | [Module lifecycle hooks](#lifecycle-hooks) |
| `ExecutionType.CpuIntensive` / `IoIntensive` | `ExecutionHint.CpuBound` / `IoBound`, including `[ExecutionHint(...)]` arguments |

Skip conditions (`WithSkipWhen`) also changed. See
[Conditions and requirements](./conditions-and-requirements.md#withskipwhen).

## Execute signatures

V3 made a generic module's result nullable for you. V4 uses the `T` you declare.

```csharp
// V3: Module<BuildOutput>, returning Task<BuildOutput?>
// V4: the result is required
public class BuildModule : Module<BuildOutput>
{
    protected override Task<BuildOutput> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
        => Task.FromResult(new BuildOutput("artifacts"));
}
```

Pick one for each module:

| The module... | Use |
| --- | --- |
| Always returns a value | `Module<BuildOutput>` and `Task<BuildOutput>` |
| Can meaningfully return `null` | `Module<BuildOutput?>` and `Task<BuildOutput?>` |
| Returns nothing | `Module` or `Module<None>` |

The same applies to `SyncModule<T>.Execute`, which now returns `T` rather than an implicit `T?`.

Non-generic modules also have renamed overrides:

| V3 | V4 |
| --- | --- |
| `Module.ExecuteModuleAsync` | `Module.ExecuteAsync` |
| `SyncModule.ExecuteModule` | `SyncModule.Execute` |

## Retries

Polly is no longer the module resilience API. A simple retry needs no policy object:

```csharp
protected override void Configure(ModuleConfigurationBuilder module) => module
    .WithRetry(
        count: 3,
        baseDelay: TimeSpan.FromSeconds(1),
        shouldRetry: exception => exception is HttpRequestException);
```

For a custom policy, rebuild its intent with a Kevlar shield:

```csharp
// using Kevlar;
protected override void Configure(ModuleConfigurationBuilder module) => module
    .WithShield(Shield.When<HttpRequestException>()
        .Retry(3, Backoff.Custom(attempt => TimeSpan.FromSeconds(attempt * attempt))));
```

Review filters, delays, and wrapped policies. Do not just swap the Polly type name.

Behavior that differs from V3:

- **Delays.** `WithRetry` uses exponential backoff with jitter, from a default base delay of
  100 ms, capped at `maxDelay` (default 30 seconds). V3's `WithRetryCount` waited
  `attempt² × 100 ms` with no jitter. `WithRetry(n)` therefore does not reproduce the old delays
  exactly. Pass `baseDelay` or use a shield when timing matters. The global
  `PipelineOptions.DefaultRetryCount` uses the same backoff.
- **Timeouts are per attempt.** V3 applied the module timeout to all attempts combined. V4 applies
  it to each attempt, with backoff outside the timeout.
- **`WithRetry` and `WithShield` cannot be combined.** Calling both on one builder, including
  through `base.Configure(module)`, throws `InvalidOperationException`. In V3 the last
  `WithRetryCount` or `WithRetryPolicy` call silently replaced earlier ones. Keep one, and put the
  retry inside the shield when you need both.

See [Retries and Resilience Shields](../how-to/retry-policy.md).

## Lifecycle hooks

Virtual hooks are still how one module customizes its own behavior. Move `WithBeforeExecute` and
`WithAfterExecute` callbacks into these overrides:

```csharp
protected override Task OnBeforeExecuteAsync(
    IModuleContext context, CancellationToken cancellationToken)
{
    context.Logger.LogInformation("Starting build");
    return Task.CompletedTask;
}

protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
    IModuleContext context, ModuleResult<string> result, CancellationToken cancellationToken)
{
    context.Logger.LogInformation("Build finished: {Status}", result.Status);
    return Task.FromResult<ModuleResult<string>?>(null); // Keep the original result.
}
```

- Do not change `OnAfterExecuteAsync` to plain `Task`. Its nullable result means "optionally
  replace the outcome."
- Before and after hooks run once around the whole retry sequence, not once per attempt.

**`SyncModule<T>`** no longer has separate synchronous lifecycle overrides. Its `Execute` method
stays synchronous, but `OnBeforeExecute`, `OnAfterExecute`, `OnSkipped`, and `OnFailed` move to the
inherited `*Async` hooks. Return `Task.CompletedTask` for synchronous work, or
`Task.FromResult<ModuleResult<T>?>(replacement)` from the after hook.

Hooks that observe *other* modules or the whole pipeline are covered in
[Hooks and plugins](./hooks-and-plugins.md).

## Generated module accessors are removed

V4 no longer generates the `GetXModule()` and `GetXModuleIfRegistered()` extension methods in
`ModularPipelines.Generated`. Use the generic APIs with the module type:

```csharp
// V3
var build = context.GetBuildModule();
var optionalBuild = context.GetBuildModuleIfRegistered();

// V4
var build = context.GetModule<BuildModule>();
var optionalBuild = context.GetModuleIfRegistered<BuildModule>();
```

- The required lookup still returns an awaitable module. The optional lookup still returns `null`
  when the module is not registered.
- Keep your dependency declarations, including `Optional = true` for optional dependencies. The
  dependency analyzer and its code fix support both generic APIs.
- If referenced libraries have modules with the same name, qualify the type or use an alias, for
  example `context.GetModule<Shared.BuildModule>()`.
- Remove static calls to the generated extension classes.
- Remove `using ModularPipelines.Generated;` only if nothing else in the file needs it. Generated
  registration, command, secret, and event metadata are still there.

## Parallel limiter targets

`ParallelLimiterAttribute<TParallelLimit>` now allows class targets only.

- Remove assembly-level and method-level uses. They compiled in V3 but never limited anything.
- Apply `[ParallelLimiter<MyParallelLimit>]` to each module class, or call
  `module.WithParallelLimiter<MyParallelLimit>()` inside `Configure`.
- Fluent configuration overrides an inherited or directly applied attribute.
- Modules that use the same limit type share its limit, whichever style they use. Planning and
  execution both honor it.

## Reading `module.Configuration`

This only matters if your code inspects a module's configuration.

- `RetryPolicyFactory`, `OnBeforeExecute`, and `OnAfterExecute` are removed.
- `ModuleConfiguration` can no longer be constructed directly.
- `SkipCondition` and `IgnoreFailuresCondition` are internal. You can no longer read or invoke
  these delegates.

Set skip and failure behavior inside `Configure(ModuleConfigurationBuilder)` instead:

- Skipping: `WithSkipWhen(...)` or `WithSkipIf(...)`.
- Ignoring failures: `WithIgnoreFailuresWhen(...)` or `WithIgnoreFailures()`.

The asynchronous `WithSkipWhen` and `WithIgnoreFailuresWhen` callbacks take a `CancellationToken`
and return `ValueTask<...>`. Synchronous overloads remain.
