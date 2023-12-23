using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-identity", "unlink-developer-identity")]
public record AwsCognitoIdentityUnlinkDeveloperIdentityOptions(
[property: CommandSwitch("--identity-id")] string IdentityId,
[property: CommandSwitch("--identity-pool-id")] string IdentityPoolId,
[property: CommandSwitch("--developer-provider-name")] string DeveloperProviderName,
[property: CommandSwitch("--developer-user-identifier")] string DeveloperUserIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}