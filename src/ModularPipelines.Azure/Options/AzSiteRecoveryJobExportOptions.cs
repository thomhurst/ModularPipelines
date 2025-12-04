using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("site-recovery", "job", "export")]
public record AzSiteRecoveryJobExportOptions : AzOptions
{
    [CliOption("--affected-object-types")]
    public string? AffectedObjectTypes { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--fabric-id")]
    public string? FabricId { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--job-output-type")]
    public string? JobOutputType { get; set; }

    [CliOption("--job-status")]
    public string? JobStatus { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--timezone-offset")]
    public string? TimezoneOffset { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}