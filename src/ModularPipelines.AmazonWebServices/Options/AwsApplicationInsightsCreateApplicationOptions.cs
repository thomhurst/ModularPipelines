using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("application-insights", "create-application")]
public record AwsApplicationInsightsCreateApplicationOptions : AwsOptions
{
    [CommandSwitch("--resource-group-name")]
    public string? ResourceGroupName { get; set; }

    [CommandSwitch("--ops-item-sns-topic-arn")]
    public string? OpsItemSnsTopicArn { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--grouping-type")]
    public string? GroupingType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}