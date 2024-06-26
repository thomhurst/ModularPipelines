using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stars")]
public record NpmStarsOptions : NpmOptions
{
    [CommandSwitch("--registry")]
    public Uri? Registry { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? User { get; set; }
}