using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "jobs", "submit", "spark-r")]
public record GcloudDataprocJobsSubmitSparkROptions(
[property: CliArgument] string RFile,
[property: CliArgument] string JobArgs,
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--cluster-labels")] IEnumerable<KeyValue> ClusterLabels
) : GcloudOptions
{
    [CliOption("--archives")]
    public string[]? Archives { get; set; }

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

    [CliOption("--files")]
    public string[]? Files { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--max-failures-per-hour")]
    public string? MaxFailuresPerHour { get; set; }

    [CliOption("--max-failures-total")]
    public string? MaxFailuresTotal { get; set; }

    [CliOption("--properties")]
    public string[]? Properties { get; set; }

    [CliOption("--properties-file")]
    public string? PropertiesFile { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}