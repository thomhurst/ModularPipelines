using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "create-anomaly-subscription")]
public record AwsCeCreateAnomalySubscriptionOptions(
[property: CliOption("--anomaly-subscription")] string AnomalySubscription
) : AwsOptions
{
    [CliOption("--resource-tags")]
    public string[]? ResourceTags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}