using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "delete-bandwidth-rate-limit")]
public record AwsStoragegatewayDeleteBandwidthRateLimitOptions(
[property: CliOption("--gateway-arn")] string GatewayArn,
[property: CliOption("--bandwidth-type")] string BandwidthType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}