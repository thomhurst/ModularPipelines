using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ce", "delete-anomaly-subscription")]
public record AwsCeDeleteAnomalySubscriptionOptions(
[property: CliOption("--subscription-arn")] string SubscriptionArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}