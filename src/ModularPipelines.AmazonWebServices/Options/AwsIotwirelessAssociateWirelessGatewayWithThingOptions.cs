using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "associate-wireless-gateway-with-thing")]
public record AwsIotwirelessAssociateWirelessGatewayWithThingOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--thing-arn")] string ThingArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}