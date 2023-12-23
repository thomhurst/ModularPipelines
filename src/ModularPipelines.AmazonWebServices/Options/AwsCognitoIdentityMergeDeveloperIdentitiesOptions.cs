using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-identity", "merge-developer-identities")]
public record AwsCognitoIdentityMergeDeveloperIdentitiesOptions(
[property: CommandSwitch("--source-user-identifier")] string SourceUserIdentifier,
[property: CommandSwitch("--destination-user-identifier")] string DestinationUserIdentifier,
[property: CommandSwitch("--developer-provider-name")] string DeveloperProviderName,
[property: CommandSwitch("--identity-pool-id")] string IdentityPoolId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}