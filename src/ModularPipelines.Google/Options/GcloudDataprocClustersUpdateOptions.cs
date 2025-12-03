using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "clusters", "update")]
public record GcloudDataprocClustersUpdateOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--graceful-decommission-timeout")]
    public string? GracefulDecommissionTimeout { get; set; }

    [CliOption("--min-secondary-worker-fraction")]
    public string? MinSecondaryWorkerFraction { get; set; }

    [CliOption("--num-secondary-workers")]
    public string? NumSecondaryWorkers { get; set; }

    [CliOption("--num-workers")]
    public string? NumWorkers { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliOption("--autoscaling-policy")]
    public string? AutoscalingPolicy { get; set; }

    [CliFlag("--disable-autoscaling")]
    public bool? DisableAutoscaling { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliOption("--expiration-time")]
    public string? ExpirationTime { get; set; }

    [CliOption("--max-age")]
    public string? MaxAge { get; set; }

    [CliFlag("--no-max-age")]
    public bool? NoMaxAge { get; set; }

    [CliOption("--max-idle")]
    public string? MaxIdle { get; set; }

    [CliFlag("--no-max-idle")]
    public bool? NoMaxIdle { get; set; }
}