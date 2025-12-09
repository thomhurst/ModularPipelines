using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-identity", "get-open-id-token-for-developer-identity")]
public record AwsCognitoIdentityGetOpenIdTokenForDeveloperIdentityOptions(
[property: CliOption("--identity-pool-id")] string IdentityPoolId,
[property: CliOption("--logins")] IEnumerable<KeyValue> Logins
) : AwsOptions
{
    [CliOption("--identity-id")]
    public string? IdentityId { get; set; }

    [CliOption("--principal-tags")]
    public IEnumerable<KeyValue>? PrincipalTags { get; set; }

    [CliOption("--token-duration")]
    public long? TokenDuration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}