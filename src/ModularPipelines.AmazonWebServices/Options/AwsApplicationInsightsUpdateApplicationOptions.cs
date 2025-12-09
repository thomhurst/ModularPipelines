using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("application-insights", "update-application")]
public record AwsApplicationInsightsUpdateApplicationOptions(
[property: CliOption("--resource-group-name")] string ResourceGroupName
) : AwsOptions
{
    [CliOption("--ops-item-sns-topic-arn")]
    public string? OpsItemSnsTopicArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}