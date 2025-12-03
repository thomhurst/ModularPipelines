using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-transit-gateway-connect")]
public record AwsEc2CreateTransitGatewayConnectOptions(
[property: CliOption("--transport-transit-gateway-attachment-id")] string TransportTransitGatewayAttachmentId,
[property: CliOption("--options")] string Options
) : AwsOptions
{
    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}