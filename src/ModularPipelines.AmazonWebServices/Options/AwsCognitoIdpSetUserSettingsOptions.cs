using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "set-user-settings")]
public record AwsCognitoIdpSetUserSettingsOptions(
[property: CliOption("--access-token")] string AccessToken,
[property: CliOption("--mfa-options")] string[] MfaOptions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}