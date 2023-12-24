using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elbv2", "modify-rule")]
public record AwsElbv2ModifyRuleOptions(
[property: CommandSwitch("--rule-arn")] string RuleArn
) : AwsOptions
{
    [CommandSwitch("--conditions")]
    public string[]? Conditions { get; set; }

    [CommandSwitch("--actions")]
    public string[]? Actions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}