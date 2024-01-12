using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "submit")]
public record GcloudBuildsSubmitOptions(
[property: PositionalArgument] string Source,
[property: PositionalArgument] string NoSource
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--no-cache")]
    public bool? NoCache { get; set; }

    [CommandSwitch("--default-buckets-behavior")]
    public string? DefaultBucketsBehavior { get; set; }

    [CommandSwitch("--dir")]
    public string? Dir { get; set; }

    [CommandSwitch("--disk-size")]
    public string? DiskSize { get; set; }

    [CommandSwitch("--gcs-log-dir")]
    public string? GcsLogDir { get; set; }

    [CommandSwitch("--gcs-source-staging-dir")]
    public string? GcsSourceStagingDir { get; set; }

    [CommandSwitch("--git-source-dir")]
    public string? GitSourceDir { get; set; }

    [CommandSwitch("--git-source-revision")]
    public string? GitSourceRevision { get; set; }

    [CommandSwitch("--ignore-file")]
    public string? IgnoreFile { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--revision")]
    public string? Revision { get; set; }

    [CommandSwitch("--substitutions")]
    public IEnumerable<KeyValue>? Substitutions { get; set; }

    [BooleanCommandSwitch("--suppress-logs")]
    public bool? SuppressLogs { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--worker-pool")]
    public string? WorkerPool { get; set; }

    [CommandSwitch("--config")]
    public string? Config { get; set; }

    [CommandSwitch("--pack")]
    public string[]? Pack { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }
}