using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel", "alert-rule", "action", "create")]
public record AzSentinelAlertRuleActionCreateOptions(
[property: CliOption("--action-name")] string ActionName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule-name")] string RuleName,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--logic-app-resource-id")]
    public string? LogicAppResourceId { get; set; }

    [CliOption("--trigger-uri")]
    public string? TriggerUri { get; set; }
}