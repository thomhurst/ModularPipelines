---
title: Context services
sidebar_position: 7
---

# Context services, build systems, and sub-modules

## Resolving services

| V3 access | V4 access |
| --- | --- |
| `context.Services.Get<T>()` | `context.Services.GetRequiredService<T>()` |
| `context.GetService<T>()` (required lookup extension) | `context.Services.GetRequiredService<T>()` |
| `context.TryGetService<T>()` | `context.Services.GetRequiredService<T>()` to keep V3 behavior, or `context.Services.GetService<T>()` if absence is allowed |

:::warning `TryGetService` threw in V3
Despite its name, V3's `TryGetService` threw when the service was missing, because it called
`GetRequiredService`. V4's `GetService` really returns `null`. Use it only where absence is
allowed, and add null handling. A blanket `Get` replacement can change runtime behavior.
:::

## Build system detection

`IBuildSystemContext` (`context.Environment.BuildSystem`) replaces one flag per CI system with
`Current` and `Is(BuildSystem)`:

```csharp
// V3
if (context.Environment.BuildSystem.IsGitHubActions) { ... }

// V4
if (context.Environment.BuildSystem.Is(BuildSystem.GitHubActions)) { ... }
```

The same applies to `IsAzurePipelines`, `IsTeamCity`, `IsJenkins`, `IsGitLab`, `IsBitbucket`,
`IsTravisCI`, and `IsAppVeyor`. `BuildSystem` is in the root `ModularPipelines` namespace.

| V3 | V4 |
| --- | --- |
| `context.Environment.BuildSystem.IsBuildServer` | `context.Environment.BuildSystem.IsCI` |
| `context.IsRunningInCI()` | `context.Environment.BuildSystem.IsCI` |
| `context.IsRunningLocally()` | `context.Environment.BuildSystem.IsLocal` |
| `context.IsRunningIn(buildSystem)` | `context.Environment.BuildSystem.Is(buildSystem)` |
| Injected `IBuildSystemDetector` | Internal; inject `IBuildSystemContext` |

Two behaviors differ:

- **`Is` checks every enum value.** V3's `IsRunningIn` handled only GitHub Actions, Azure
  Pipelines, TeamCity, and Jenkins, and returned `false` for anything else. V4's `Is` compares
  against `Current` for every value, including `BuildSystem.Unknown`. Review callers that relied
  on the old fallback.
- **`IsCI` also honors the `CI` environment variable.** It is `true` when `CI` is set to a truthy
  value, not only on a recognized build agent. `OnCI`, `OnLocal`, and `Require.Ci()` share this one
  definition, so they always agree. As a result, `CI=0` counts as local, and `CI=false` no longer
  satisfies `Require.Ci()`.

When the `CI` variable is the only reason a run counts as CI, V4 logs this once at startup:
`Treating this run as CI because CI=<value> and no known build agent was detected.`

## Sub-modules

Sub-modules still work, and you can still name and run them from runtime logic, such as a loop over
discovered projects. Only the method name and the body signature changed:

```csharp
// V3
await context.SubModule("Upload", () => UploadAsync(cancellationToken));

// V4
await context.RunSubModuleAsync("Upload", token => UploadAsync(token), cancellationToken);
```

- Both the value-returning and non-value-returning overloads take a token-aware body. Forward the
  token given to the body into the operation.
- Each sub-module still appears by name in progress output.
- Failures propagate their original exception. `SubModuleFailedException` is removed.
- `SubModuleBase`, the internal progress-tracking type, is no longer public. This does not affect
  calls to `RunSubModuleAsync`.

See [Sub-modules](../how-to/sub-modules.md).
