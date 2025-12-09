using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-vpc-endpoint-connection-notifications")]
public record AwsEc2DeleteVpcEndpointConnectionNotificationsOptions(
[property: CliOption("--connection-notification-ids")] string[] ConnectionNotificationIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}