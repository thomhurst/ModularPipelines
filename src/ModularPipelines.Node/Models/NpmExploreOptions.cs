using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("explore")]
public record NpmExploreOptions : NpmOptions
{
    [CommandSwitch("--shell")]
    public string? Shell { get; set; }

}