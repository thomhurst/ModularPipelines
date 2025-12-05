using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("xray", "delete-sampling-rule")]
public record AwsXrayDeleteSamplingRuleOptions : AwsOptions
{
    [CliOption("--rule-name")]
    public string? RuleName { get; set; }

    [CliOption("--rule-arn")]
    public string? RuleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}