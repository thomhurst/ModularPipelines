using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("site-recovery", "job", "export")]
public record AzSiteRecoveryJobExportOptions : AzOptions
{
    [CommandSwitch("--affected-object-types")]
    public string? AffectedObjectTypes { get; set; }

    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--fabric-id")]
    public string? FabricId { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--job-name")]
    public string? JobName { get; set; }

    [CommandSwitch("--job-output-type")]
    public string? JobOutputType { get; set; }

    [CommandSwitch("--job-status")]
    public string? JobStatus { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--timezone-offset")]
    public string? TimezoneOffset { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }
}