using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "instances", "update")]
public record GcloudAlloydbInstancesUpdateOptions(
[property: CliArgument] string Instance,
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--region")] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--availability-type")]
    public string? AvailabilityType { get; set; }

    [CliOption("--cpu-count")]
    public string? CpuCount { get; set; }

    [CliOption("--database-flags")]
    public string[]? DatabaseFlags { get; set; }

    [CliOption("--insights-config-query-plans-per-minute")]
    public string? InsightsConfigQueryPlansPerMinute { get; set; }

    [CliOption("--insights-config-query-string-length")]
    public string? InsightsConfigQueryStringLength { get; set; }

    [CliOption("--[no-]insights-config-record-application-tags")]
    public string[]? NoInsightsConfigRecordApplicationTags { get; set; }

    [CliOption("--[no-]insights-config-record-client-address")]
    public string[]? NoInsightsConfigRecordClientAddress { get; set; }

    [CliOption("--read-pool-node-count")]
    public string? ReadPoolNodeCount { get; set; }

    [CliOption("--[no-]require-connectors")]
    public string[]? NoRequireConnectors { get; set; }

    [CliOption("--ssl-mode")]
    public string? SslMode { get; set; }
}