using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hdinsight", "host", "restart")]
public record AzHdinsightHostRestartOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--host-names")] string HostNames,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}