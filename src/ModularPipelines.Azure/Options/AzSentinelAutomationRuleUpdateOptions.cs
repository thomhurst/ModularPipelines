using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sentinel", "automation-rule", "update")]
public record AzSentinelAutomationRuleUpdateOptions : AzOptions
{
    [CliOption("--actions")]
    public string? Actions { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--automation-rule-name")]
    public string? AutomationRuleName { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--order")]
    public string? Order { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--triggering-logic")]
    public string? TriggeringLogic { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}