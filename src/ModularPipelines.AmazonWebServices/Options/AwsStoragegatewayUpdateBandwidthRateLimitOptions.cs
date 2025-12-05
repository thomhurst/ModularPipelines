using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "update-bandwidth-rate-limit")]
public record AwsStoragegatewayUpdateBandwidthRateLimitOptions(
[property: CliOption("--gateway-arn")] string GatewayArn
) : AwsOptions
{
    [CliOption("--average-upload-rate-limit-in-bits-per-sec")]
    public long? AverageUploadRateLimitInBitsPerSec { get; set; }

    [CliOption("--average-download-rate-limit-in-bits-per-sec")]
    public long? AverageDownloadRateLimitInBitsPerSec { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}