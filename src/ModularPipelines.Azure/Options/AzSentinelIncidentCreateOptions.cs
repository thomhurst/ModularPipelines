using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "incident", "create")]
public record AzSentinelIncidentCreateOptions(
[property: CommandSwitch("--incident-id")] string IncidentId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [BooleanCommandSwitch("--classification")]
    public bool? Classification { get; set; }

    [CommandSwitch("--classification-comment")]
    public string? ClassificationComment { get; set; }

    [CommandSwitch("--classification-reason")]
    public string? ClassificationReason { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--first-activity-time-utc")]
    public string? FirstActivityTimeUtc { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--last-activity-time-utc")]
    public string? LastActivityTimeUtc { get; set; }

    [CommandSwitch("--owner")]
    public string? Owner { get; set; }

    [CommandSwitch("--provider-incident-id")]
    public string? ProviderIncidentId { get; set; }

    [CommandSwitch("--provider-name")]
    public string? ProviderName { get; set; }

    [CommandSwitch("--severity")]
    public string? Severity { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--title")]
    public string? Title { get; set; }
}