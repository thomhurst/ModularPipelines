using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "associate-wireless-gateway-with-thing")]
public record AwsIotwirelessAssociateWirelessGatewayWithThingOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--thing-arn")] string ThingArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}