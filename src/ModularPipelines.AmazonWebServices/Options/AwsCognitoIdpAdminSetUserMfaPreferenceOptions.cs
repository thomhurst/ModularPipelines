using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "admin-set-user-mfa-preference")]
public record AwsCognitoIdpAdminSetUserMfaPreferenceOptions(
[property: CommandSwitch("--username")] string Username,
[property: CommandSwitch("--user-pool-id")] string UserPoolId
) : AwsOptions
{
    [CommandSwitch("--sms-mfa-settings")]
    public string? SmsMfaSettings { get; set; }

    [CommandSwitch("--software-token-mfa-settings")]
    public string? SoftwareTokenMfaSettings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}