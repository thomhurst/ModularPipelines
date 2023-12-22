using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access", "set")]
public record NpmAccessSetOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Value
) : NpmOptions
{
    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [CommandSwitch("--otp")]
    public string? Otp { get; set; }

    [CommandSwitch("--registry")]
    public Uri? Registry { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Package { get; set; }
}