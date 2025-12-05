using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "verify-software-token")]
public record AwsCognitoIdpVerifySoftwareTokenOptions(
[property: CliOption("--user-code")] string UserCode
) : AwsOptions
{
    [CliOption("--access-token")]
    public string? AccessToken { get; set; }

    [CliOption("--session")]
    public string? Session { get; set; }

    [CliOption("--friendly-device-name")]
    public string? FriendlyDeviceName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}