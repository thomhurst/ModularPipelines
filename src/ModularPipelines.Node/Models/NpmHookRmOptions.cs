using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hook", "rm")]
public record NpmHookRmOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Id
) : NpmOptions
{
    [CommandSwitch("--registry")]
    public virtual Uri? Registry { get; set; }

    [CommandSwitch("--otp")]
    public virtual string? Otp { get; set; }
}