using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudfront", "delete-monitoring-subscription")]
public record AwsCloudfrontDeleteMonitoringSubscriptionOptions(
[property: CliOption("--distribution-id")] string DistributionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}