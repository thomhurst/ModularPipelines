using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "update-bandwidth-rate-limit-schedule")]
public record AwsStoragegatewayUpdateBandwidthRateLimitScheduleOptions(
[property: CliOption("--gateway-arn")] string GatewayArn,
[property: CliOption("--bandwidth-rate-limit-intervals")] string[] BandwidthRateLimitIntervals
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}