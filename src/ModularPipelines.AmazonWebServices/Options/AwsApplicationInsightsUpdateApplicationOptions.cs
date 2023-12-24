using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("application-insights", "update-application")]
public record AwsApplicationInsightsUpdateApplicationOptions(
[property: CommandSwitch("--resource-group-name")] string ResourceGroupName
) : AwsOptions
{
    [CommandSwitch("--ops-item-sns-topic-arn")]
    public string? OpsItemSnsTopicArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}