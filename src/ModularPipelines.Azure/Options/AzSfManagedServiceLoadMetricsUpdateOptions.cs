using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "managed-service", "load-metrics", "update")]
public record AzSfManagedServiceLoadMetricsUpdateOptions(
[property: CommandSwitch("--application")] string Application,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--metric-name")] string MetricName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--default-load")]
    public string? DefaultLoad { get; set; }

    [CommandSwitch("--primary-default-load")]
    public string? PrimaryDefaultLoad { get; set; }

    [CommandSwitch("--secondary-default-load")]
    public string? SecondaryDefaultLoad { get; set; }

    [CommandSwitch("--weight")]
    public string? Weight { get; set; }
}