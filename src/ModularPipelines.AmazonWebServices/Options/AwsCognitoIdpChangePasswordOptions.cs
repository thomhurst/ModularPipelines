using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "change-password")]
public record AwsCognitoIdpChangePasswordOptions(
[property: CommandSwitch("--previous-password")] string PreviousPassword,
[property: CommandSwitch("--proposed-password")] string ProposedPassword,
[property: CommandSwitch("--access-token")] string AccessToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}