using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "jobs", "submit", "presto")]
public record GcloudDataprocJobsSubmitPrestoOptions(
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--cluster-labels")] IEnumerable<KeyValue> ClusterLabels,
[property: CommandSwitch("--execute")] string Execute,
[property: CommandSwitch("--file")] string File
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--bucket")]
    public string? Bucket { get; set; }

    [CommandSwitch("--client-tags")]
    public string[]? ClientTags { get; set; }

    [BooleanCommandSwitch("--continue-on-failure")]
    public bool? ContinueOnFailure { get; set; }

    [CommandSwitch("--driver-log-levels")]
    public string[]? DriverLogLevels { get; set; }

    [CommandSwitch("--driver-required-memory-mb")]
    public string? DriverRequiredMemoryMb { get; set; }

    [CommandSwitch("--driver-required-vcores")]
    public string? DriverRequiredVcores { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--max-failures-per-hour")]
    public string? MaxFailuresPerHour { get; set; }

    [CommandSwitch("--max-failures-total")]
    public string? MaxFailuresTotal { get; set; }

    [CommandSwitch("--properties")]
    public string[]? Properties { get; set; }

    [CommandSwitch("--properties-file")]
    public string? PropertiesFile { get; set; }

    [CommandSwitch("--query-output-format")]
    public string? QueryOutputFormat { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}