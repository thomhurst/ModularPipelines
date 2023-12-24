using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "delete-monitoring-subscription")]
public record AwsCloudfrontDeleteMonitoringSubscriptionOptions(
[property: CommandSwitch("--distribution-id")] string DistributionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}