using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("profile", "set")]
public record NpmProfileSetOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Key,
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Value
) : NpmOptions
{
    [CommandSwitch("--registry")]
    public virtual Uri? Registry { get; set; }

    [BooleanCommandSwitch("--json")]
    public virtual bool? Json { get; set; }

    [BooleanCommandSwitch("--parseable")]
    public virtual bool? Parseable { get; set; }

    [CommandSwitch("--otp")]
    public virtual string? Otp { get; set; }
}