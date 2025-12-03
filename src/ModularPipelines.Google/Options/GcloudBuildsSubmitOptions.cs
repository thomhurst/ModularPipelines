using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "submit")]
public record GcloudBuildsSubmitOptions(
[property: CliArgument] string Source,
[property: CliArgument] string NoSource
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--no-cache")]
    public bool? NoCache { get; set; }

    [CliOption("--default-buckets-behavior")]
    public string? DefaultBucketsBehavior { get; set; }

    [CliOption("--dir")]
    public string? Dir { get; set; }

    [CliOption("--disk-size")]
    public string? DiskSize { get; set; }

    [CliOption("--gcs-log-dir")]
    public string? GcsLogDir { get; set; }

    [CliOption("--gcs-source-staging-dir")]
    public string? GcsSourceStagingDir { get; set; }

    [CliOption("--git-source-dir")]
    public string? GitSourceDir { get; set; }

    [CliOption("--git-source-revision")]
    public string? GitSourceRevision { get; set; }

    [CliOption("--ignore-file")]
    public string? IgnoreFile { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--revision")]
    public string? Revision { get; set; }

    [CliOption("--substitutions")]
    public IEnumerable<KeyValue>? Substitutions { get; set; }

    [CliFlag("--suppress-logs")]
    public bool? SuppressLogs { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--worker-pool")]
    public string? WorkerPool { get; set; }

    [CliOption("--config")]
    public string? Config { get; set; }

    [CliOption("--pack")]
    public string[]? Pack { get; set; }

    [CliOption("--tag")]
    public string? Tag { get; set; }
}