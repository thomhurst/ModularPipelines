using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetOptions() : CommandLineToolOptions("dotnet")
{
    [PositionalArgument]
    public string? TargetPath { get; init; }

    [CommandEqualsSeparatorSwitch("--runtime", SwitchValueSeparator = " ")]
    public string? Runtime { get; init; }

    [CommandSwitch("--verbosity")]
    public Verbosity? Verbosity { get; init; }

    [CommandEqualsSeparatorSwitch("--property", SwitchValueSeparator = ":")]
    public IEnumerable<string>? Properties { get; init; }
}