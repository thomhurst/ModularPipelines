using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "get-identity-provider-by-identifier")]
public record AwsCognitoIdpGetIdentityProviderByIdentifierOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--idp-identifier")] string IdpIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}