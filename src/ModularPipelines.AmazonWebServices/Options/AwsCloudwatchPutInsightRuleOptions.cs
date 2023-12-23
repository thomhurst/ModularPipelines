using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudwatch", "put-insight-rule")]
public record AwsCloudwatchPutInsightRuleOptions(
[property: CommandSwitch("--rule-name")] string RuleName,
[property: CommandSwitch("--rule-definition")] string RuleDefinition
) : AwsOptions
{
    [CommandSwitch("--rule-state")]
    public string? RuleState { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}