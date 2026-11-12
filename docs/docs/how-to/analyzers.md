---
title: Analyzers
---

# Analyzers

Modular Pipelines includes Roslyn analyzers that catch pipeline authoring mistakes at compile time. Each rule uses the `MP####` ID family, so it can be configured with standard .NET analyzer settings.

See the [complete analyzer rule reference](../analyzers/) for every rule, its category, default severity, and suppression examples.

## Dependency access and cancellation

[`MP0021`](../analyzers/MP0021.md) warns when an optional dependency is accessed with
`GetModule<T>()`. Use `GetModuleIfRegistered<T>()` and handle an absent module.
[`MP0022`](../analyzers/MP0022.md) is informational: a required dependency can use
`GetModule<T>()` instead of an unnecessary nullable lookup. Both rules inspect
`GetModule<T>()` and `GetModuleIfRegistered<T>()` calls. Declarations include inherited
`DependsOn` attributes and fluent `DependsOn` / `DependsOnOptional` calls in `Configure`; a required
declaration wins when the same dependency is declared more than once. Conditional
fluent declarations with an unknown condition do not produce accessor mismatch diagnostics.

[`MP0016`](../analyzers/MP0016.md) checks cancellation forwarding in `ExecuteAsync`,
its reachable helpers, and lifecycle overrides: `OnBeforeExecuteAsync`,
`OnAfterExecuteAsync`, `OnSkippedAsync`, `OnCachedResultAsync`, and `OnFailedAsync`.
It also checks asynchronous module configuration callbacks that receive a
`CancellationToken`, including `WithSkipWhen`, `WithSkipWhenAll`, and
`WithIgnoreFailuresWhen`. Pass the available token to asynchronous operations:

```csharp
protected override void Configure(ModuleConfigurationBuilder module) =>
    module.WithSkipWhen(async (context, cancellationToken) =>
    {
        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        return false;
    });
```

Unrelated methods with the same names and callbacks without an available token
are excluded.

### What MP0016 reports {#mp0016-scope}

MP0016 reports an awaited or directly returned call that has an accessible
`CancellationToken` overload when either:

- no token is passed, or
- every token passed is lost on every path that reaches the call.

A lost token is one that cannot carry module cancellation:

| Value | Lost |
|-------|------|
| `default`, `CancellationToken.None`, `new CancellationToken(...)` | Yes |
| `new CancellationTokenSource(...).Token` | Yes |
| A local or parameter that was assigned a lost value on every path | Yes |
| `a ?? b`, `c ? a : b`, or a switch expression whose results are all lost | Yes |
| Anything else | No |

```csharp
using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(1));
await Task.Delay(1, timeout.Token); // MP0016: the module token is not linked

using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
linked.CancelAfter(TimeSpan.FromMinutes(1));
await Task.Delay(1, linked.Token); // no diagnostic
```

When the analyzer cannot cheaply prove that a token is lost, it stays silent. These
shapes are never followed and never produce a diagnostic for a passed token:

- `ref`, `out`, and `in` arguments, ref locals, and ref returns
- arrays, spans, tuples, deconstruction, and collections
- fields, properties, and awaited values, including `Task` and `ValueTask` results
- the result or body of any method, local function, or lambda that is given a token
- a captured variable that is assigned anywhere after its declaration
- a `CancellationTokenSource` variable that is used for anything other than assigning it or reading or calling its own members, such as capturing it, passing it to a method, or copying it
- a variable written inside a `try` block, as seen from its `catch` or `finally`
- branches that are unreachable only because of a constant condition

This scope is fixed. A missed diagnostic in one of these shapes is by design.

## Module registration

[`MP0013`](../analyzers/MP0013.md) warns when an executable project declares a concrete
module that nothing registers. Library projects are not checked.

### What MP0013 reports {#mp0013-scope}

A module counts as registered when any of these appears anywhere in the project:

- `AddModule`, `AddModules`, `AddModulesFromAssembly`, or
  `AddModulesFromAssemblyContainingType` naming the module or its assembly
- an `IModule` service registration for it on an `IServiceCollection`
- `ModuleTester.For<TModule>()...ExecuteAsync()`
- a required `DependsOn` from a module that is registered

```csharp
var command = new RootCommand();
command.SetAction(_ => Pipeline.CreateBuilder().AddModule<BuildModule>()); // BuildModule is registered
return command.Parse(args).Invoke();
```

The analyzer does not check whether the registration runs. A registration in a
callback, command action, helper, or branch counts even if that code is never
reached, so a module registered only in dead code is not reported. When a
registration's modules cannot be determined at compile time, such as a `Type[]`
computed at run time, MP0013 stays silent for the whole project.

This scope is fixed. A missed diagnostic for a registration that never executes is by design.

## Use pipeline services inside modules

These warnings apply to code declared in module types, including constructors,
field initializers, lifecycle methods, helpers, and callbacks. Nested helper types
that do not inherit from a module are excluded.

- [`MP0024`](../analyzers/MP0024.md) warns on `System.Diagnostics.Process.Start`
  (static or instance calls and method-group references) and `new ProcessStartInfo(...)`. Use
  `context.Shell.RunAsync(...)` so commands participate in secret masking,
  cancellation, command interception, and `ModuleTester` command recording.
- [`MP0025`](../analyzers/MP0025.md) warns on `new HttpClient(...)`, including
  target-typed `new()`. Use `context.Network.Http.SendAsync(...)` for pipeline
  resilience, logging, and `InterceptHttp`. Injecting an existing configured
  client is allowed.
- [`MP0026`](../analyzers/MP0026.md) warns when a module constructor receives
  another module, including primary constructors and parameters typed as
  `IModule`. Declare the dependency with `DependsOn` and retrieve it through
  `context.GetModule<T>()` or `context.GetModuleIfRegistered<T>()`. The warning
  applies even when the dependency is already declared, so accesses remain
  visible to dependency analysis.

For example, execute a command and read a dependency through the context:

```csharp
[DependsOn<BuildModule>]
public class PublishModule : Module<int>
{
    protected override async Task<int> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        var build = await context.GetModule<BuildModule>();
        await context.Shell.RunAsync("tool", ["publish"], cancellationToken);
        return 0;
    }
}
```

Suppress an individual warning when direct access is intentional.

## Configure a rule

### Optional console policy

[`MP0004`](../analyzers/MP0004.md) is disabled by default. Direct `System.Console`
output is supported in both modules and `Program.cs`. Enable this rule only when
your project requires structured logging instead of console stream output:

```ini
[*.cs]
dotnet_diagnostic.MP0004.severity = warning
```

When enabled, it reports direct `System.Console` calls throughout the project.
The code fix can replace supported module `WriteLine` calls with `context.Logger`
events. This changes output semantics: events use logging levels, filters, and
providers instead of plain stdout/stderr capture. No automatic replacement is
offered in `Program.cs`, where a module context is unavailable. See
[Choosing an output API](./logging.md#choosing-an-output-api).

### Severity

Set a rule's severity in `.editorconfig`:

```ini
[*.cs]
dotnet_diagnostic.MP0001.severity = warning
```

Valid values are `default`, `none`, `silent`, `suggestion`, `warning`, and `error`. Use `none` to disable a rule.

## Suppress a specific occurrence

Use `#pragma` when a violation is intentional and narrowly scoped:

```csharp
#pragma warning disable MP0001
// Code that intentionally violates MP0001.
#pragma warning restore MP0001
```

## ID migration

The legacy prose IDs were renamed so `.editorconfig` settings can use one predictable family:

| Legacy ID | Current ID |
|-----------|------------|
| `MissingDependsOnAttribute` | `MP0001` |
| `EnumerableModuleResult` | `MP0002` |
| `LoggerInConstructor` | `MP0003` |
| `ConsoleUse` | `MP0004` |
| `ConflictingDependsOnAttribute` | `MP0005` |
| `AsyncModule` | `MP0006` |
| `AwaitThis` | `MP0007` |
| `StatefulModule` | `MP0008` |
| `MPDEP001` | `MP0009` |
| `MPDEP002` | `MP0005` |
| `MPDEP003` | `MP0010` |
| `MPREG001` | `MP0013` |
| `MPASYNC001` | `MP0014` |
| `MPASYNC002` | `MP0015` |
| `MPASYNC003` | `MP0016` |
| `MPASYNC004` | `MP0017` |
| `MPTYPE001` | `MP0018` |
| `MPDEP004` | `MP0019` |

Update existing `dotnet_diagnostic.<id>.severity` entries and warning suppressions to the current IDs.

## V3-to-V4 upgrade code fixes

After upgrading the core and integration packages to V4, use **Upgrade to
ModularPipelines V4** from your editor's quick actions on compiler errors
`CS1061`, `CS0117`, or `CS1739`. These fixes cover:

- Tool access, such as `context.Git()` becoming `context.Tools.Git`.
- Methods renamed with an `Async` suffix.
- Named arguments renamed from `token:` to `cancellationToken:`.
- Git commands moved into a command group, discovered from the installed `IGitCommands` API.
- Legacy `ModuleStatus` names, including `Successful` becoming `Succeeded` and `Processing` becoming `Running`.

For example, apply the tool-access fix and then the command fix to migrate:

```csharp
await context.Git().Commands.Config(options, token: cancellationToken);
```

into:

```csharp
using ModularPipelines.Context;

await context.Tools.Git.Commands.Repository.ConfigAsync(options, cancellationToken: cancellationToken);
```

Fixes bind each proposed replacement against the APIs installed in your project.
A replacement is offered only when it resolves without errors to a ModularPipelines
member. Ambiguous Git groups and calls whose arguments also need manual changes
are left for you to resolve. Import `ModularPipelines.Context`, where the source
generator emits the tool extension properties; an old import such as
`ModularPipelines.Git.Extensions` does not expose them. Extension properties require a
C# 14-capable editor/compiler.

Use **Fix All** for independent occurrences. A chain of old calls can expose a
new compiler error after the first fix; apply the next quick action or repeat
Fix All until the mechanical changes are complete. Review the resulting code:
these fixes do not add `await`, change option types, register integrations, or
handle every V4 breaking change.
