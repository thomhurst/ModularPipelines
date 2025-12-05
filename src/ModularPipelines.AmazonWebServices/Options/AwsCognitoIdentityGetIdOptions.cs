using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-identity", "get-id")]
public record AwsCognitoIdentityGetIdOptions(
[property: CliOption("--identity-pool-id")] string IdentityPoolId
) : AwsOptions
{
    [CliOption("--account-id")]
    public string? AccountId { get; set; }

    [CliOption("--logins")]
    public IEnumerable<KeyValue>? Logins { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}