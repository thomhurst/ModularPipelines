using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "update-gateway-information")]
public record AwsStoragegatewayUpdateGatewayInformationOptions(
[property: CliOption("--gateway-arn")] string GatewayArn
) : AwsOptions
{
    [CliOption("--gateway-name")]
    public string? GatewayName { get; set; }

    [CliOption("--gateway-timezone")]
    public string? GatewayTimezone { get; set; }

    [CliOption("--cloud-watch-log-group-arn")]
    public string? CloudWatchLogGroupArn { get; set; }

    [CliOption("--gateway-capacity")]
    public string? GatewayCapacity { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}