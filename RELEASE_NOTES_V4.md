# ModularPipelines V4 Release Notes

## Installers

The core installer surface now contains only generic local and web installation:

```csharp
await context.Installers.InstallAsync(new InstallerOptions("./setup.sh"));
await context.Installers.InstallFromWebAsync(new WebInstallerOptions(downloadUri));
```

`context.Installers.File`, the Windows, Linux, macOS, and predefined installer
contexts, and their platform-specific option types have been removed. Use the
dedicated Brew, Chocolatey, Winget, Node, or other tool integration instead of the
removed core package-manager wrappers.

## Logging options

Per-call command and HTTP options now use `Logging`, matching the global
`Commands.Logging` and `Http.Logging` properties. `IncludeTimestamps` is now
`ShowTimestamps`.

`HttpLoggingType` and `HttpOptions.LoggingType` were removed. Configure request,
response, status-code, duration, header, and body logging through
`HttpLoggingOptions`. `CommandLogVerbosity.Minimal` means command input only.

The unused `PipelineCommandOptions.Execution` property was removed. Continue to pass
execution behavior through `CommandExecutionOptions` on each command call.

## Failure modes and execution hints

Pipeline failure behavior now uses `FailureMode` instead of `ExecutionMode`:

- `ExecutionMode.StopOnFirstException` is now `FailureMode.FailFast`.
- `ExecutionMode.WaitForAllModules` is now `FailureMode.ContinueOnFailure`.
- `PipelineOptions.ExecutionMode` is now `PipelineOptions.FailureMode`.

Module resource classification now uses `ExecutionHint` instead of `ExecutionType`:

- `ExecutionType.CpuIntensive` is now `ExecutionHint.CpuBound`.
- `ExecutionType.IoIntensive` is now `ExecutionHint.IoBound`.
- `ModuleConfiguration.ExecutionType` is now `ModuleConfiguration.ExecutionHint`.
- `WithExecutionHint(ExecutionType)` now accepts `ExecutionHint`.

The `[ExecutionHint(...)]` attribute syntax is unchanged.

## Module result metadata

`IModuleResult` and `ModuleResult` now use concise metadata names:

- `ModuleName` is now `Name`.
- `ModuleTypeName` is now `TypeName`.
- `ModuleDuration` is now `Duration`.
- `ModuleStart` is now `StartTime`.
- `ModuleEnd` is now `EndTime`.

The custom JSON converters use the same new property names. Consumers of persisted or
distributed `ModuleResult` JSON must migrate those five field names together with the
.NET API.

## Module condition predicates

`WithSkipWhen` now has boolean predicate overloads that accept a skip reason. Use
`SkipDecision.When(bool, string?)` when constructing a decision directly.
`SkipDecision.Of(bool, string?)` remains as an obsolete compatibility alias.

Asynchronous module predicates now consistently use `ValueTask`. The
`ModuleConfiguration.IgnoreFailuresCondition` property and the asynchronous
`ModuleConfigurationBuilder.WithIgnoreFailuresWhen` overload therefore accept
`Func<IModuleContext, Exception, ValueTask<bool>>` instead of the previous
`Task<bool>` delegate. Explicitly typed callers must migrate inside their module's
configuration hook for v4:

```csharp
protected override void Configure(ModuleConfigurationBuilder module)
{
    Func<IModuleContext, Exception, ValueTask<bool>> ignoreFailure =
        (context, exception) => ValueTask.FromResult(exception is ApiValidationException);

    module.WithIgnoreFailuresWhen(ignoreFailure);
}
```

## Pipeline builder creation

`Pipeline.CreateBuilder(args)` now infers the pipeline project directory from the
calling source file. `Pipeline.CreateBuilderFromSource` has been removed; use
`CreateBuilder` for both inferred and explicitly configured builders.

`PipelineBuilderOptions` is now `PipelineBuilderSettings`. The build-time assembly
discovery flag moved from `PipelineOptions.LoadModularPipelineAssemblies` to
`PipelineBuilderSettings.LoadModularPipelinesAssemblies`:

```csharp
var builder = Pipeline.CreateBuilder(new PipelineBuilderSettings
{
    Args = args,
    LoadModularPipelinesAssemblies = true,
});
```

`PipelineBuilder` no longer implements `IDisposable`; its previous `Dispose` method
performed no cleanup. Remove `using` declarations around builders. Resources created
by a successful build remain owned by the resulting pipeline.

## CLI argument ordering

`CommandLinePhase` is now the only ordering model for flags, options, and arguments.
`ArgumentPlacement` has been removed. Migrate custom positional arguments as follows:

```csharp
// V3
[CliArgument(0, Placement = ArgumentPlacement.BeforeOptions)]
public string? Path { get; init; }

// V4
[CliArgument(0, Phase = CommandLinePhase.EarlyOperand)]
public string? Path { get; init; }
```

Generated option types now emit an explicit phase for every positional argument.
Operands documented before an `[OPTIONS]` or `[flags]` token use `EarlyOperand`; operands
after that token use `Passthrough`. Generated command-line sequences otherwise remain
unchanged.

The complete subcommand chain is atomic. `EarlyOperand` values render after the final
subcommand, while properties inherited from a `[CliGlobalOptions]` type remain the only
values that render before the first subcommand.

Intentional behavior changes:

- A hand-written `[CliArgument]` defaults to `Passthrough`. The generator model's
  `CliPositionalArgument.Phase` defaults to `EarlyOperand`, so generator extensions
  that omit the phase place operands before normal options.
- A contradictory placement/phase combination can no longer silently ignore its phase,
  because `ArgumentPlacement` no longer exists.

## Canonical key-value CLI options

Generated key-value option properties now consistently use `IReadOnlyList<KeyValue>?`.
Properties previously emitted as `KeyValue[]?` or `IEnumerable<KeyValue>?` are
source-breaking only for callers that depend on those exact declared types. Collection
expressions and `KeyValue` tuple conversions continue to work:

```csharp
var options = new DockerRunOptions
{
    Annotation = [("owner", "platform"), ("environment", "ci")],
};
```

Two-operand options now use `CliValuePair` instead of `CliOptionValuePair`:

```csharp
var options = new JqExecuteOptions
{
    Arg = [new CliValuePair("name", "Ada")],
};
```

`CliValuePair` options must use a space separator because each occurrence renders as
`--option first second`. Other `OptionFormat` values now throw during command construction
instead of being silently ignored.

## CLI argument placeholders removed

`CliArgumentAttribute.Name` and `<PLACEHOLDER>` substitution in command parts have
been removed. Every `[CliArgument]` value is now rendered as a positional argument;
it can no longer disappear because a matching placeholder was absent.

For a command chain that depends on constructor input, compute `CommandParts`
explicitly:

```csharp
// Before: Action disappeared when <ACTION> was missing or did not match.
[CliCommand("tool", "resource", "<ACTION>")]
public record ResourceOptions(
    [property: CliArgument(0, Name = "<ACTION>")] string Action)
    : CommandLineToolOptions;

// V4: the constructor owns the dynamic command chain.
[CliTool("tool")]
public record ResourceOptions : CommandLineToolOptions
{
    public ResourceOptions(string action)
    {
        CommandParts = ["resource", action];
    }
}
```

Use `[CliArgument]` only for positional values that follow the command chain.
