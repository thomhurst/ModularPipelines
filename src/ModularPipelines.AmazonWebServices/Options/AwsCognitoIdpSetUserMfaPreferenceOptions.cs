using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "set-user-mfa-preference")]
public record AwsCognitoIdpSetUserMfaPreferenceOptions(
[property: CliOption("--access-token")] string AccessToken
) : AwsOptions
{
    [CliOption("--sms-mfa-settings")]
    public string? SmsMfaSettings { get; set; }

    [CliOption("--software-token-mfa-settings")]
    public string? SoftwareTokenMfaSettings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}