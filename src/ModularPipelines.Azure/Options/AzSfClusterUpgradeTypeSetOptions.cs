using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "cluster", "upgrade-type", "set")]
public record AzSfClusterUpgradeTypeSetOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--upgrade-mode")] string UpgradeMode
) : AzOptions
{
    [CliOption("--version")]
    public string? Version { get; set; }
}