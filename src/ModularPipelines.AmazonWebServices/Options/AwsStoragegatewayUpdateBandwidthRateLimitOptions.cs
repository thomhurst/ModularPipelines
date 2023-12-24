using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "update-bandwidth-rate-limit")]
public record AwsStoragegatewayUpdateBandwidthRateLimitOptions(
[property: CommandSwitch("--gateway-arn")] string GatewayArn
) : AwsOptions
{
    [CommandSwitch("--average-upload-rate-limit-in-bits-per-sec")]
    public long? AverageUploadRateLimitInBitsPerSec { get; set; }

    [CommandSwitch("--average-download-rate-limit-in-bits-per-sec")]
    public long? AverageDownloadRateLimitInBitsPerSec { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}