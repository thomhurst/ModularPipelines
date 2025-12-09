using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "create-user-pool")]
public record AwsCognitoIdpCreateUserPoolOptions(
[property: CliOption("--pool-name")] string PoolName
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

    [CliOption("--alias-attributes")]
    public string[]? AliasAttributes { get; set; }

    [CliOption("--username-attributes")]
    public string[]? UsernameAttributes { get; set; }

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

    [CliOption("--mfa-configuration")]
    public string? MfaConfiguration { get; set; }

    [CliOption("--user-attribute-update-settings")]
    public string? UserAttributeUpdateSettings { get; set; }

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

    [CliOption("--schema")]
    public string[]? Schema { get; set; }

    [CliOption("--user-pool-add-ons")]
    public string? UserPoolAddOns { get; set; }

    [CliOption("--username-configuration")]
    public string? UsernameConfiguration { get; set; }

    [CliOption("--account-recovery-setting")]
    public string? AccountRecoverySetting { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}