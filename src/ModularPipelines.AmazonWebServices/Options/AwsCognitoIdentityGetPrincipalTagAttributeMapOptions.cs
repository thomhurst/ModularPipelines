using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-identity", "get-principal-tag-attribute-map")]
public record AwsCognitoIdentityGetPrincipalTagAttributeMapOptions(
[property: CliOption("--identity-pool-id")] string IdentityPoolId,
[property: CliOption("--identity-provider-name")] string IdentityProviderName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}