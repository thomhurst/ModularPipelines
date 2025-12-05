using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudwatch", "put-insight-rule")]
public record AwsCloudwatchPutInsightRuleOptions(
[property: CliOption("--rule-name")] string RuleName,
[property: CliOption("--rule-definition")] string RuleDefinition
) : AwsOptions
{
    [CliOption("--rule-state")]
    public string? RuleState { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}