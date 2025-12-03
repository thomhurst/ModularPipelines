using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sentinel", "incident", "create")]
public record AzSentinelIncidentCreateOptions(
[property: CliOption("--incident-id")] string IncidentId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
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

    [CliOption("--severity")]
    public string? Severity { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }
}