using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-identity", "set-principal-tag-attribute-map")]
public record AwsCognitoIdentitySetPrincipalTagAttributeMapOptions(
[property: CommandSwitch("--identity-pool-id")] string IdentityPoolId,
[property: CommandSwitch("--identity-provider-name")] string IdentityProviderName
) : AwsOptions
{
    [CommandSwitch("--principal-tags")]
    public IEnumerable<KeyValue>? PrincipalTags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}