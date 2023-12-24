using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "activate-gateway")]
public record AwsStoragegatewayActivateGatewayOptions(
[property: CommandSwitch("--activation-key")] string ActivationKey,
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--gateway-timezone")] string GatewayTimezone,
[property: CommandSwitch("--gateway-region")] string GatewayRegion
) : AwsOptions
{
    [CommandSwitch("--gateway-type")]
    public string? GatewayType { get; set; }

    [CommandSwitch("--tape-drive-type")]
    public string? TapeDriveType { get; set; }

    [CommandSwitch("--medium-changer-type")]
    public string? MediumChangerType { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}