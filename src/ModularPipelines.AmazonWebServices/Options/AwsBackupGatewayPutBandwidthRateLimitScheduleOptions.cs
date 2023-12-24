using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup-gateway", "put-bandwidth-rate-limit-schedule")]
public record AwsBackupGatewayPutBandwidthRateLimitScheduleOptions(
[property: CommandSwitch("--bandwidth-rate-limit-intervals")] string[] BandwidthRateLimitIntervals,
[property: CommandSwitch("--gateway-arn")] string GatewayArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}