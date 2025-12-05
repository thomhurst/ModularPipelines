using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediaconnect", "update-gateway-instance")]
public record AwsMediaconnectUpdateGatewayInstanceOptions(
[property: CliOption("--gateway-instance-arn")] string GatewayInstanceArn
) : AwsOptions
{
    [CliOption("--bridge-placement")]
    public string? BridgePlacement { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}