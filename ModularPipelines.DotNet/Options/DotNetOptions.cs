using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.DotNet.Options;

public record DotNetOptions() : CommandLineToolOptions("dotnet")
{
    public string? TargetPath { get; init; }

    [CommandLongSwitch("runtime", SwitchValueSeparator = " ")]
    public string? Runtime { get; init; }

    [CommandSwitch("v")]
    public Verbosity? Verbosity { get; init; }

    [CommandLongSwitch("property", SwitchValueSeparator = ":")]
    public string[]? Properties { get; init; }
}
