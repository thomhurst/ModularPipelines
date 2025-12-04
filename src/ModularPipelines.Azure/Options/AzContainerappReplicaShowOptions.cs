using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "replica", "show")]
public record AzContainerappReplicaShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--replica")] string Replica,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--revision")]
    public string? Revision { get; set; }
}