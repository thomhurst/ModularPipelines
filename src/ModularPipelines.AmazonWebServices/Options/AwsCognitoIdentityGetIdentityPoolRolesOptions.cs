using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-identity", "get-identity-pool-roles")]
public record AwsCognitoIdentityGetIdentityPoolRolesOptions(
[property: CommandSwitch("--identity-pool-id")] string IdentityPoolId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}