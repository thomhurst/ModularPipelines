using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks", "mesh", "upgrade", "start")]
public record AzAksMeshUpgradeStartOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--revision")] string Revision
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}