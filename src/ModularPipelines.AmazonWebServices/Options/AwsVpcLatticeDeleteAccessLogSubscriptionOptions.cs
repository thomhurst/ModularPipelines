using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vpc-lattice", "delete-access-log-subscription")]
public record AwsVpcLatticeDeleteAccessLogSubscriptionOptions(
[property: CliOption("--access-log-subscription-identifier")] string AccessLogSubscriptionIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}