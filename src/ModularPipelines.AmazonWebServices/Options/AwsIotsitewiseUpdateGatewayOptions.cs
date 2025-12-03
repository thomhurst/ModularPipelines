using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "update-gateway")]
public record AwsIotsitewiseUpdateGatewayOptions(
[property: CliOption("--gateway-id")] string GatewayId,
[property: CliOption("--gateway-name")] string GatewayName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}