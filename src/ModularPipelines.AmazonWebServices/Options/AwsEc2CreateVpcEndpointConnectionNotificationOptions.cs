using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-vpc-endpoint-connection-notification")]
public record AwsEc2CreateVpcEndpointConnectionNotificationOptions(
[property: CommandSwitch("--connection-notification-arn")] string ConnectionNotificationArn,
[property: CommandSwitch("--connection-events")] string[] ConnectionEvents
) : AwsOptions
{
    [CommandSwitch("--service-id")]
    public string? ServiceId { get; set; }

    [CommandSwitch("--vpc-endpoint-id")]
    public string? VpcEndpointId { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}