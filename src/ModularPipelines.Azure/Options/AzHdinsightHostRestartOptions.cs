using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight", "host", "restart")]
public record AzHdinsightHostRestartOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--host-names")] string HostNames,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}