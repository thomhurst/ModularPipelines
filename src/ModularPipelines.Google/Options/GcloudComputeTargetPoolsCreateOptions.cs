using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-pools", "create")]
public record GcloudComputeTargetPoolsCreateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--backup-pool")]
    public string? BackupPool { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--failover-ratio")]
    public string? FailoverRatio { get; set; }

    [CliOption("--health-check")]
    public string? HealthCheck { get; set; }

    [CliOption("--http-health-check")]
    public string? HttpHealthCheck { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--session-affinity")]
    public string? SessionAffinity { get; set; }
}