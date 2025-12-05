using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-identity", "merge-developer-identities")]
public record AwsCognitoIdentityMergeDeveloperIdentitiesOptions(
[property: CliOption("--source-user-identifier")] string SourceUserIdentifier,
[property: CliOption("--destination-user-identifier")] string DestinationUserIdentifier,
[property: CliOption("--developer-provider-name")] string DeveloperProviderName,
[property: CliOption("--identity-pool-id")] string IdentityPoolId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}