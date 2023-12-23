using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "set-user-pool-mfa-config")]
public record AwsCognitoIdpSetUserPoolMfaConfigOptions(
[property: CommandSwitch("--user-pool-id")] string UserPoolId
) : AwsOptions
{
    [CommandSwitch("--sms-mfa-configuration")]
    public string? SmsMfaConfiguration { get; set; }

    [CommandSwitch("--software-token-mfa-configuration")]
    public string? SoftwareTokenMfaConfiguration { get; set; }

    [CommandSwitch("--mfa-configuration")]
    public string? MfaConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}