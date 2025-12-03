using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "admin-set-user-mfa-preference")]
public record AwsCognitoIdpAdminSetUserMfaPreferenceOptions(
[property: CliOption("--username")] string Username,
[property: CliOption("--user-pool-id")] string UserPoolId
) : AwsOptions
{
    [CliOption("--sms-mfa-settings")]
    public string? SmsMfaSettings { get; set; }

    [CliOption("--software-token-mfa-settings")]
    public string? SoftwareTokenMfaSettings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}