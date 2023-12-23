using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "confirm-device")]
public record AwsCognitoIdpConfirmDeviceOptions(
[property: CommandSwitch("--access-token")] string AccessToken,
[property: CommandSwitch("--device-key")] string DeviceKey
) : AwsOptions
{
    [CommandSwitch("--device-secret-verifier-config")]
    public string? DeviceSecretVerifierConfig { get; set; }

    [CommandSwitch("--device-name")]
    public string? DeviceName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}