using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hook", "add")]
public record NpmHookAddOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Pkg,
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Url,
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Secret
) : NpmOptions
{
    [CommandSwitch("--registry")]
    public Uri? Registry { get; set; }

    [CommandSwitch("--otp")]
    public string? Otp { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Type { get; set; }
}