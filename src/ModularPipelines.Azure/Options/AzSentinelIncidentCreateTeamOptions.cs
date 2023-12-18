using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "incident", "create-team")]
public record AzSentinelIncidentCreateTeamOptions(
[property: CommandSwitch("--incident-id")] string IncidentId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--team-name")] string TeamName,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--group-ids")]
    public string? GroupIds { get; set; }

    [CommandSwitch("--member-ids")]
    public string? MemberIds { get; set; }

    [CommandSwitch("--team-description")]
    public string? TeamDescription { get; set; }
}