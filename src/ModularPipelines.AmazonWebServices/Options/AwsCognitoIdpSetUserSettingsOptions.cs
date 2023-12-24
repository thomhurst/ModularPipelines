using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "set-user-settings")]
public record AwsCognitoIdpSetUserSettingsOptions(
[property: CommandSwitch("--access-token")] string AccessToken,
[property: CommandSwitch("--mfa-options")] string[] MfaOptions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}