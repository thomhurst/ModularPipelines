using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-vpc-endpoint-connection-notifications")]
public record AwsEc2DeleteVpcEndpointConnectionNotificationsOptions(
[property: CommandSwitch("--connection-notification-ids")] string[] ConnectionNotificationIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}