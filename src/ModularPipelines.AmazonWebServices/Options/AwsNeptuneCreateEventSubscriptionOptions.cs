using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune", "create-event-subscription")]
public record AwsNeptuneCreateEventSubscriptionOptions(
[property: CliOption("--subscription-name")] string SubscriptionName,
[property: CliOption("--sns-topic-arn")] string SnsTopicArn
) : AwsOptions
{
    [CliOption("--source-type")]
    public string? SourceType { get; set; }

    [CliOption("--event-categories")]
    public string[]? EventCategories { get; set; }

    [CliOption("--source-ids")]
    public string[]? SourceIds { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}