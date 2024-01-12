using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "jobs", "submit", "spark-r")]
public record GcloudDataprocJobsSubmitSparkROptions(
[property: PositionalArgument] string RFile,
[property: PositionalArgument] string JobArgs,
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--cluster-labels")] IEnumerable<KeyValue> ClusterLabels
) : GcloudOptions
{
    [CommandSwitch("--archives")]
    public string[]? Archives { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--bucket")]
    public string? Bucket { get; set; }

    [CommandSwitch("--driver-log-levels")]
    public string[]? DriverLogLevels { get; set; }

    [CommandSwitch("--driver-required-memory-mb")]
    public string? DriverRequiredMemoryMb { get; set; }

    [CommandSwitch("--driver-required-vcores")]
    public string? DriverRequiredVcores { get; set; }

    [CommandSwitch("--files")]
    public string[]? Files { get; set; }

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

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}