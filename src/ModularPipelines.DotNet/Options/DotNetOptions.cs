using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.DotNet.Options;

public record DotNetOptions() : CommandLineToolOptions("dotnet")
{
    [PositionalArgument]
    public string? TargetPath { get; init; }

    [CommandEqualsSeparatorSwitch("--runtime", SwitchValueSeparator = " ")]
    public string? Runtime { get; init; }

    [CommandSwitch("-v")]
    public Verbosity? Verbosity { get; init; }

    [CommandEqualsSeparatorSwitch("--property", SwitchValueSeparator = ":")]
    public IEnumerable<string>? Properties { get; init; }
}
