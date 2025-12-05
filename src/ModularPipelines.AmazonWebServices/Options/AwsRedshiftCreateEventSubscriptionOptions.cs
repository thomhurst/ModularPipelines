using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "create-event-subscription")]
public record AwsRedshiftCreateEventSubscriptionOptions(
[property: CliOption("--subscription-name")] string SubscriptionName,
[property: CliOption("--sns-topic-arn")] string SnsTopicArn
) : AwsOptions
{
    [CliOption("--source-type")]
    public string? SourceType { get; set; }

    [CliOption("--source-ids")]
    public string[]? SourceIds { get; set; }

    [CliOption("--event-categories")]
    public string[]? EventCategories { get; set; }

    [CliOption("--severity")]
    public string? Severity { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}