using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "update-device-status")]
public record AwsCognitoIdpUpdateDeviceStatusOptions(
[property: CommandSwitch("--access-token")] string AccessToken,
[property: CommandSwitch("--device-key")] string DeviceKey
) : AwsOptions
{
    [CommandSwitch("--device-remembered-status")]
    public string? DeviceRememberedStatus { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}