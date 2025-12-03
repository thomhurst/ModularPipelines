using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "update-user-pool")]
public record AwsCognitoIdpUpdateUserPoolOptions(
[property: CliOption("--user-pool-id")] string UserPoolId
) : AwsOptions
{
    [CliOption("--policies")]
    public string? Policies { get; set; }

    [CliOption("--deletion-protection")]
    public string? DeletionProtection { get; set; }

    [CliOption("--lambda-config")]
    public string? LambdaConfig { get; set; }

    [CliOption("--auto-verified-attributes")]
    public string[]? AutoVerifiedAttributes { get; set; }

    [CliOption("--sms-verification-message")]
    public string? SmsVerificationMessage { get; set; }

    [CliOption("--email-verification-message")]
    public string? EmailVerificationMessage { get; set; }

    [CliOption("--email-verification-subject")]
    public string? EmailVerificationSubject { get; set; }

    [CliOption("--verification-message-template")]
    public string? VerificationMessageTemplate { get; set; }

    [CliOption("--sms-authentication-message")]
    public string? SmsAuthenticationMessage { get; set; }

    [CliOption("--user-attribute-update-settings")]
    public string? UserAttributeUpdateSettings { get; set; }

    [CliOption("--mfa-configuration")]
    public string? MfaConfiguration { get; set; }

    [CliOption("--device-configuration")]
    public string? DeviceConfiguration { get; set; }

    [CliOption("--email-configuration")]
    public string? EmailConfiguration { get; set; }

    [CliOption("--sms-configuration")]
    public string? SmsConfiguration { get; set; }

    [CliOption("--user-pool-tags")]
    public IEnumerable<KeyValue>? UserPoolTags { get; set; }

    [CliOption("--admin-create-user-config")]
    public string? AdminCreateUserConfig { get; set; }

    [CliOption("--user-pool-add-ons")]
    public string? UserPoolAddOns { get; set; }

    [CliOption("--account-recovery-setting")]
    public string? AccountRecoverySetting { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}