using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "alert-rule", "action", "create")]
public record AzSentinelAlertRuleActionCreateOptions(
[property: CommandSwitch("--action-name")] string ActionName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rule-name")] string RuleName,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--logic-app-resource-id")]
    public string? LogicAppResourceId { get; set; }

    [CommandSwitch("--trigger-uri")]
    public string? TriggerUri { get; set; }
}