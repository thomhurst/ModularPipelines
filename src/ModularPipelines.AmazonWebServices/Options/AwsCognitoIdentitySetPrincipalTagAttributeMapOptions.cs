using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-identity", "set-principal-tag-attribute-map")]
public record AwsCognitoIdentitySetPrincipalTagAttributeMapOptions(
[property: CliOption("--identity-pool-id")] string IdentityPoolId,
[property: CliOption("--identity-provider-name")] string IdentityProviderName
) : AwsOptions
{
    [CliOption("--principal-tags")]
    public IEnumerable<KeyValue>? PrincipalTags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}