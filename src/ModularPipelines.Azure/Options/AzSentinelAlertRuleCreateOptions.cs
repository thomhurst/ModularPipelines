using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sentinel", "alert-rule", "create")]
public record AzSentinelAlertRuleCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--fusion")]
    public string? Fusion { get; set; }

    [CliOption("--ml-behavior-analytics")]
    public string? MlBehaviorAnalytics { get; set; }

    [CliOption("--ms-security-incident")]
    public string? MsSecurityIncident { get; set; }

    [CliOption("--nrt")]
    public string? Nrt { get; set; }

    [CliOption("--scheduled")]
    public string? Scheduled { get; set; }

    [CliOption("--threat-intelligence")]
    public string? ThreatIntelligence { get; set; }
}