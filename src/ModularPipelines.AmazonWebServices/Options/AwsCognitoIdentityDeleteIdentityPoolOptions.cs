using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-identity", "delete-identity-pool")]
public record AwsCognitoIdentityDeleteIdentityPoolOptions(
[property: CliOption("--identity-pool-id")] string IdentityPoolId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}