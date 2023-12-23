using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "create-automation-rule")]
public record AwsSecurityhubCreateAutomationRuleOptions(
[property: CommandSwitch("--rule-order")] int RuleOrder,
[property: CommandSwitch("--rule-name")] string RuleName,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--criteria")] string Criteria,
[property: CommandSwitch("--actions")] string[] Actions
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--rule-status")]
    public string? RuleStatus { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}