using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel", "alert-rule", "update")]
public record AzSentinelAlertRuleUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--fusion")]
    public string? Fusion { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--ml-behavior-analytics")]
    public string? MlBehaviorAnalytics { get; set; }

    [CliOption("--ms-security-incident")]
    public string? MsSecurityIncident { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--nrt")]
    public string? Nrt { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scheduled")]
    public string? Scheduled { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--threat-intelligence")]
    public string? ThreatIntelligence { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}