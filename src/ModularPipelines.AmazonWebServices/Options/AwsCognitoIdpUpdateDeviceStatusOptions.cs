using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "update-device-status")]
public record AwsCognitoIdpUpdateDeviceStatusOptions(
[property: CliOption("--access-token")] string AccessToken,
[property: CliOption("--device-key")] string DeviceKey
) : AwsOptions
{
    [CliOption("--device-remembered-status")]
    public string? DeviceRememberedStatus { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}