# ModularPipelines V4 Release Notes

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
