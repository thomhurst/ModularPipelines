using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("xray", "delete-sampling-rule")]
public record AwsXrayDeleteSamplingRuleOptions : AwsOptions
{
    [CommandSwitch("--rule-name")]
    public string? RuleName { get; set; }

    [CommandSwitch("--rule-arn")]
    public string? RuleArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}