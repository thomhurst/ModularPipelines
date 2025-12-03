using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-identity", "get-open-id-token")]
public record AwsCognitoIdentityGetOpenIdTokenOptions(
[property: CliOption("--identity-id")] string IdentityId
) : AwsOptions
{
    [CliOption("--logins")]
    public IEnumerable<KeyValue>? Logins { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}