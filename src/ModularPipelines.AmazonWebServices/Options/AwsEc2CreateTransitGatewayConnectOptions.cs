using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-transit-gateway-connect")]
public record AwsEc2CreateTransitGatewayConnectOptions(
[property: CommandSwitch("--transport-transit-gateway-attachment-id")] string TransportTransitGatewayAttachmentId,
[property: CommandSwitch("--options")] string Options
) : AwsOptions
{
    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}