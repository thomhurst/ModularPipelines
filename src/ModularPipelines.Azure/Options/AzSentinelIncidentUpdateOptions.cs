using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel", "incident", "update")]
public record AzSentinelIncidentUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--classification")]
    public bool? Classification { get; set; }

    [CliOption("--classification-comment")]
    public string? ClassificationComment { get; set; }

    [CliOption("--classification-reason")]
    public string? ClassificationReason { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--first-activity-time-utc")]
    public string? FirstActivityTimeUtc { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--incident-id")]
    public string? IncidentId { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliOption("--last-activity-time-utc")]
    public string? LastActivityTimeUtc { get; set; }

    [CliOption("--owner")]
    public string? Owner { get; set; }

    [CliOption("--provider-incident-id")]
    public string? ProviderIncidentId { get; set; }

    [CliOption("--provider-name")]
    public string? ProviderName { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--severity")]
    public string? Severity { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}