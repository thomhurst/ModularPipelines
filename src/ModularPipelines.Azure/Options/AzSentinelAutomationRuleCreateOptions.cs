using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel", "automation-rule", "create")]
public record AzSentinelAutomationRuleCreateOptions(
[property: CliOption("--automation-rule-name")] string AutomationRuleName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--actions")]
    public string? Actions { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--order")]
    public string? Order { get; set; }

    [CliOption("--triggering-logic")]
    public string? TriggeringLogic { get; set; }
}