using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "start-availability-monitor-test")]
public record AwsStoragegatewayStartAvailabilityMonitorTestOptions(
[property: CliOption("--gateway-arn")] string GatewayArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}