using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edit")]
public record NpmEditOptions : NpmOptions
{
    [CommandSwitch("--editor")]
    public string? Editor { get; set; }

}