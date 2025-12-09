using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-identity", "list-identities")]
public record AwsCognitoIdentityListIdentitiesOptions(
[property: CliOption("--identity-pool-id")] string IdentityPoolId,
[property: CliOption("--max-results")] int MaxResults
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}