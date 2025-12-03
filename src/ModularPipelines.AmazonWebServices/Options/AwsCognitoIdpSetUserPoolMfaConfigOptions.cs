using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "set-user-pool-mfa-config")]
public record AwsCognitoIdpSetUserPoolMfaConfigOptions(
[property: CliOption("--user-pool-id")] string UserPoolId
) : AwsOptions
{
    [CliOption("--sms-mfa-configuration")]
    public string? SmsMfaConfiguration { get; set; }

    [CliOption("--software-token-mfa-configuration")]
    public string? SoftwareTokenMfaConfiguration { get; set; }

    [CliOption("--mfa-configuration")]
    public string? MfaConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}