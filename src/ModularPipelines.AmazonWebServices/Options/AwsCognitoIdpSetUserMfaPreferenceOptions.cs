using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "set-user-mfa-preference")]
public record AwsCognitoIdpSetUserMfaPreferenceOptions(
[property: CommandSwitch("--access-token")] string AccessToken
) : AwsOptions
{
    [CommandSwitch("--sms-mfa-settings")]
    public string? SmsMfaSettings { get; set; }

    [CommandSwitch("--software-token-mfa-settings")]
    public string? SoftwareTokenMfaSettings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}