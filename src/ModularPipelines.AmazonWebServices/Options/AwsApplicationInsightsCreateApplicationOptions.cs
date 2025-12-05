using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("application-insights", "create-application")]
public record AwsApplicationInsightsCreateApplicationOptions : AwsOptions
{
    [CliOption("--resource-group-name")]
    public string? ResourceGroupName { get; set; }

    [CliOption("--ops-item-sns-topic-arn")]
    public string? OpsItemSnsTopicArn { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--grouping-type")]
    public string? GroupingType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}