using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "set-smb-guest-password")]
public record AwsStoragegatewaySetSmbGuestPasswordOptions(
[property: CommandSwitch("--gateway-arn")] string GatewayArn,
[property: CommandSwitch("--password")] string Password
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}