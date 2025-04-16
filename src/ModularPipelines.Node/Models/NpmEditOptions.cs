using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edit")]
public record NpmEditOptions : NpmOptions
{
    [CommandSwitch("--editor")]
    public virtual string? Editor { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Pkg { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string? Subpkg { get; set; }
}