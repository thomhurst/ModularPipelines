using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "alert-rule", "create")]
public record AzSentinelAlertRuleCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--fusion")]
    public string? Fusion { get; set; }

    [CommandSwitch("--ml-behavior-analytics")]
    public string? MlBehaviorAnalytics { get; set; }

    [CommandSwitch("--ms-security-incident")]
    public string? MsSecurityIncident { get; set; }

    [CommandSwitch("--nrt")]
    public string? Nrt { get; set; }

    [CommandSwitch("--scheduled")]
    public string? Scheduled { get; set; }

    [CommandSwitch("--threat-intelligence")]
    public string? ThreatIntelligence { get; set; }
}