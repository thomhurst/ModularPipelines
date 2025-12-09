using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel", "incident", "create-team")]
public record AzSentinelIncidentCreateTeamOptions(
[property: CliOption("--incident-id")] string IncidentId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--team-name")] string TeamName,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--group-ids")]
    public string? GroupIds { get; set; }

    [CliOption("--member-ids")]
    public string? MemberIds { get; set; }

    [CliOption("--team-description")]
    public string? TeamDescription { get; set; }
}