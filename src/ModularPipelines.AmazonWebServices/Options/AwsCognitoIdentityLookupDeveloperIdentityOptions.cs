using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-identity", "lookup-developer-identity")]
public record AwsCognitoIdentityLookupDeveloperIdentityOptions(
[property: CliOption("--identity-pool-id")] string IdentityPoolId
) : AwsOptions
{
    [CliOption("--identity-id")]
    public string? IdentityId { get; set; }

    [CliOption("--developer-user-identifier")]
    public string? DeveloperUserIdentifier { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}