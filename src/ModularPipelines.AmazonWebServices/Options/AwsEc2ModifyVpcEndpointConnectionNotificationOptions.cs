using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-vpc-endpoint-connection-notification")]
public record AwsEc2ModifyVpcEndpointConnectionNotificationOptions(
[property: CommandSwitch("--connection-notification-id")] string ConnectionNotificationId
) : AwsOptions
{
    [CommandSwitch("--connection-notification-arn")]
    public string? ConnectionNotificationArn { get; set; }

    [CommandSwitch("--connection-events")]
    public string[]? ConnectionEvents { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}