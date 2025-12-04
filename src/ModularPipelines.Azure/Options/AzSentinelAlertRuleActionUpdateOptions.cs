using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel", "alert-rule", "action", "update")]
public record AzSentinelAlertRuleActionUpdateOptions : AzOptions
{
    [CliOption("--action-name")]
    public string? ActionName { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--logic-app-resource-id")]
    public string? LogicAppResourceId { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rule-name")]
    public string? RuleName { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--trigger-uri")]
    public string? TriggerUri { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}