using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "automation-rule", "create")]
public record AzSentinelAutomationRuleCreateOptions(
[property: CommandSwitch("--automation-rule-name")] string AutomationRuleName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CommandSwitch("--actions")]
    public string? Actions { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--order")]
    public string? Order { get; set; }

    [CommandSwitch("--triggering-logic")]
    public string? TriggeringLogic { get; set; }
}