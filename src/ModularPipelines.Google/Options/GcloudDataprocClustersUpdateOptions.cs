using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "clusters", "update")]
public record GcloudDataprocClustersUpdateOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--graceful-decommission-timeout")]
    public string? GracefulDecommissionTimeout { get; set; }

    [CommandSwitch("--min-secondary-worker-fraction")]
    public string? MinSecondaryWorkerFraction { get; set; }

    [CommandSwitch("--num-secondary-workers")]
    public string? NumSecondaryWorkers { get; set; }

    [CommandSwitch("--num-workers")]
    public string? NumWorkers { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CommandSwitch("--autoscaling-policy")]
    public string? AutoscalingPolicy { get; set; }

    [BooleanCommandSwitch("--disable-autoscaling")]
    public bool? DisableAutoscaling { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CommandSwitch("--expiration-time")]
    public string? ExpirationTime { get; set; }

    [CommandSwitch("--max-age")]
    public string? MaxAge { get; set; }

    [BooleanCommandSwitch("--no-max-age")]
    public bool? NoMaxAge { get; set; }

    [CommandSwitch("--max-idle")]
    public string? MaxIdle { get; set; }

    [BooleanCommandSwitch("--no-max-idle")]
    public bool? NoMaxIdle { get; set; }
}