using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vpc-lattice", "update-access-log-subscription")]
public record AwsVpcLatticeUpdateAccessLogSubscriptionOptions(
[property: CliOption("--access-log-subscription-identifier")] string AccessLogSubscriptionIdentifier,
[property: CliOption("--destination-arn")] string DestinationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}