using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "activate-gateway")]
public record AwsStoragegatewayActivateGatewayOptions(
[property: CliOption("--activation-key")] string ActivationKey,
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--gateway-timezone")] string GatewayTimezone,
[property: CliOption("--gateway-region")] string GatewayRegion
) : AwsOptions
{
    [CliOption("--gateway-type")]
    public string? GatewayType { get; set; }

    [CliOption("--tape-drive-type")]
    public string? TapeDriveType { get; set; }

    [CliOption("--medium-changer-type")]
    public string? MediumChangerType { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}