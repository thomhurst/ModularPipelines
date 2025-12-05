using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup-gateway", "put-bandwidth-rate-limit-schedule")]
public record AwsBackupGatewayPutBandwidthRateLimitScheduleOptions(
[property: CliOption("--bandwidth-rate-limit-intervals")] string[] BandwidthRateLimitIntervals,
[property: CliOption("--gateway-arn")] string GatewayArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}