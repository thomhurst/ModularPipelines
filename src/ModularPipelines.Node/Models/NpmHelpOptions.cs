using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("help")]
public record NpmHelpOptions(
    [property: PositionalArgument(Position = Position.BeforeSwitches)] string Term
) : NpmOptions
{
    [CommandSwitch("--viewer")]
    public virtual string? Viewer { get; set; }
}