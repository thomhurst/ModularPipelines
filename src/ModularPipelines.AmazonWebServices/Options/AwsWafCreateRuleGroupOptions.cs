using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf", "create-rule-group")]
public record AwsWafCreateRuleGroupOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--metric-name")] string MetricName,
[property: CommandSwitch("--change-token")] string ChangeToken
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}