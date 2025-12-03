using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "jobs", "submit", "spark-sql")]
public record GcloudDataprocJobsSubmitSparkSqlOptions(
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--cluster-labels")] IEnumerable<KeyValue> ClusterLabels,
[property: CliOption("--execute")] string Execute,
[property: CliOption("--file")] string File
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--bucket")]
    public string? Bucket { get; set; }

    [CliOption("--driver-log-levels")]
    public string[]? DriverLogLevels { get; set; }

    [CliOption("--driver-required-memory-mb")]
    public string? DriverRequiredMemoryMb { get; set; }

    [CliOption("--driver-required-vcores")]
    public string? DriverRequiredVcores { get; set; }

    [CliOption("--jars")]
    public string[]? Jars { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--max-failures-per-hour")]
    public string? MaxFailuresPerHour { get; set; }

    [CliOption("--max-failures-total")]
    public string? MaxFailuresTotal { get; set; }

    [CliOption("--params")]
    public string[]? Params { get; set; }

    [CliOption("--properties")]
    public string[]? Properties { get; set; }

    [CliOption("--properties-file")]
    public string? PropertiesFile { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}