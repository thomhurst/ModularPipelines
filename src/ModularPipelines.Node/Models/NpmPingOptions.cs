using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("ping")]
public record NpmPingOptions : NpmOptions
{
    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }
}