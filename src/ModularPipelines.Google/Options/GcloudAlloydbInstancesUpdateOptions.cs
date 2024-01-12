using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alloydb", "instances", "update")]
public record GcloudAlloydbInstancesUpdateOptions(
[property: PositionalArgument] string Instance,
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--availability-type")]
    public string? AvailabilityType { get; set; }

    [CommandSwitch("--cpu-count")]
    public string? CpuCount { get; set; }

    [CommandSwitch("--database-flags")]
    public string[]? DatabaseFlags { get; set; }

    [CommandSwitch("--insights-config-query-plans-per-minute")]
    public string? InsightsConfigQueryPlansPerMinute { get; set; }

    [CommandSwitch("--insights-config-query-string-length")]
    public string? InsightsConfigQueryStringLength { get; set; }

    [CommandSwitch("--[no-]insights-config-record-application-tags")]
    public string[]? NoInsightsConfigRecordApplicationTags { get; set; }

    [CommandSwitch("--[no-]insights-config-record-client-address")]
    public string[]? NoInsightsConfigRecordClientAddress { get; set; }

    [CommandSwitch("--read-pool-node-count")]
    public string? ReadPoolNodeCount { get; set; }

    [CommandSwitch("--[no-]require-connectors")]
    public string[]? NoRequireConnectors { get; set; }

    [CommandSwitch("--ssl-mode")]
    public string? SslMode { get; set; }
}