using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "get-device")]
public record AwsCognitoIdpGetDeviceOptions(
[property: CliOption("--device-key")] string DeviceKey
) : AwsOptions
{
    [CliOption("--access-token")]
    public string? AccessToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}