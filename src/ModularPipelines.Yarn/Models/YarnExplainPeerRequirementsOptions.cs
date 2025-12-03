using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("explain", "peer-requirements")]
public record YarnExplainPeerRequirementsOptions : YarnOptions
{
    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string? Hash { get; set; }
}