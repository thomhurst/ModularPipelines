using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "disassociate-traffic-distribution-group-user")]
public record AwsConnectDisassociateTrafficDistributionGroupUserOptions(
[property: CliOption("--traffic-distribution-group-id")] string TrafficDistributionGroupId,
[property: CliOption("--user-id")] string UserId,
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}