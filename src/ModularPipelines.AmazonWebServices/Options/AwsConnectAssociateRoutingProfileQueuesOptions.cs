using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "associate-routing-profile-queues")]
public record AwsConnectAssociateRoutingProfileQueuesOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--routing-profile-id")] string RoutingProfileId,
[property: CliOption("--queue-configs")] string[] QueueConfigs
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}