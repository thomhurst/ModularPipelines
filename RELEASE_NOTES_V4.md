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
