using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "create-event-subscription")]
public record AwsRedshiftCreateEventSubscriptionOptions(
[property: CommandSwitch("--subscription-name")] string SubscriptionName,
[property: CommandSwitch("--sns-topic-arn")] string SnsTopicArn
) : AwsOptions
{
    [CommandSwitch("--source-type")]
    public string? SourceType { get; set; }

    [CommandSwitch("--source-ids")]
    public string[]? SourceIds { get; set; }

    [CommandSwitch("--event-categories")]
    public string[]? EventCategories { get; set; }

    [CommandSwitch("--severity")]
    public string? Severity { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}