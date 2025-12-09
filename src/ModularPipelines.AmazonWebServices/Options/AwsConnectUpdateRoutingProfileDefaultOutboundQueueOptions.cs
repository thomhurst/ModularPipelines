using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-routing-profile-default-outbound-queue")]
public record AwsConnectUpdateRoutingProfileDefaultOutboundQueueOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--routing-profile-id")] string RoutingProfileId,
[property: CliOption("--default-outbound-queue-id")] string DefaultOutboundQueueId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}