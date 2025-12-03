using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "modify-rule")]
public record AwsElbv2ModifyRuleOptions(
[property: CliOption("--rule-arn")] string RuleArn
) : AwsOptions
{
    [CliOption("--conditions")]
    public string[]? Conditions { get; set; }

    [CliOption("--actions")]
    public string[]? Actions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}