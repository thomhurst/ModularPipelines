using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-pools", "create")]
public record GcloudComputeTargetPoolsCreateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--backup-pool")]
    public string? BackupPool { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--failover-ratio")]
    public string? FailoverRatio { get; set; }

    [CommandSwitch("--health-check")]
    public string? HealthCheck { get; set; }

    [CommandSwitch("--http-health-check")]
    public string? HttpHealthCheck { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--session-affinity")]
    public string? SessionAffinity { get; set; }
}