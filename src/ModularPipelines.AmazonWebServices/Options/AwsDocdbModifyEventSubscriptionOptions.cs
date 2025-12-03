using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("docdb", "modify-event-subscription")]
public record AwsDocdbModifyEventSubscriptionOptions(
[property: CliOption("--subscription-name")] string SubscriptionName
) : AwsOptions
{
    [CliOption("--sns-topic-arn")]
    public string? SnsTopicArn { get; set; }

    [CliOption("--source-type")]
    public string? SourceType { get; set; }

    [CliOption("--event-categories")]
    public string[]? EventCategories { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}