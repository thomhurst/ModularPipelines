using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "verify-software-token")]
public record AwsCognitoIdpVerifySoftwareTokenOptions(
[property: CommandSwitch("--user-code")] string UserCode
) : AwsOptions
{
    [CommandSwitch("--access-token")]
    public string? AccessToken { get; set; }

    [CommandSwitch("--session")]
    public string? Session { get; set; }

    [CommandSwitch("--friendly-device-name")]
    public string? FriendlyDeviceName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}