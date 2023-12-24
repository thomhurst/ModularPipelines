using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("neptune", "create-event-subscription")]
public record AwsNeptuneCreateEventSubscriptionOptions(
[property: CommandSwitch("--subscription-name")] string SubscriptionName,
[property: CommandSwitch("--sns-topic-arn")] string SnsTopicArn
) : AwsOptions
{
    [CommandSwitch("--source-type")]
    public string? SourceType { get; set; }

    [CommandSwitch("--event-categories")]
    public string[]? EventCategories { get; set; }

    [CommandSwitch("--source-ids")]
    public string[]? SourceIds { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}