using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "update-user-pool")]
public record AwsCognitoIdpUpdateUserPoolOptions(
[property: CommandSwitch("--user-pool-id")] string UserPoolId
) : AwsOptions
{
    [CommandSwitch("--policies")]
    public string? Policies { get; set; }

    [CommandSwitch("--deletion-protection")]
    public string? DeletionProtection { get; set; }

    [CommandSwitch("--lambda-config")]
    public string? LambdaConfig { get; set; }

    [CommandSwitch("--auto-verified-attributes")]
    public string[]? AutoVerifiedAttributes { get; set; }

    [CommandSwitch("--sms-verification-message")]
    public string? SmsVerificationMessage { get; set; }

    [CommandSwitch("--email-verification-message")]
    public string? EmailVerificationMessage { get; set; }

    [CommandSwitch("--email-verification-subject")]
    public string? EmailVerificationSubject { get; set; }

    [CommandSwitch("--verification-message-template")]
    public string? VerificationMessageTemplate { get; set; }

    [CommandSwitch("--sms-authentication-message")]
    public string? SmsAuthenticationMessage { get; set; }

    [CommandSwitch("--user-attribute-update-settings")]
    public string? UserAttributeUpdateSettings { get; set; }

    [CommandSwitch("--mfa-configuration")]
    public string? MfaConfiguration { get; set; }

    [CommandSwitch("--device-configuration")]
    public string? DeviceConfiguration { get; set; }

    [CommandSwitch("--email-configuration")]
    public string? EmailConfiguration { get; set; }

    [CommandSwitch("--sms-configuration")]
    public string? SmsConfiguration { get; set; }

    [CommandSwitch("--user-pool-tags")]
    public IEnumerable<KeyValue>? UserPoolTags { get; set; }

    [CommandSwitch("--admin-create-user-config")]
    public string? AdminCreateUserConfig { get; set; }

    [CommandSwitch("--user-pool-add-ons")]
    public string? UserPoolAddOns { get; set; }

    [CommandSwitch("--account-recovery-setting")]
    public string? AccountRecoverySetting { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}