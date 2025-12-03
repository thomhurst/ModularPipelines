using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-identity", "unlink-developer-identity")]
public record AwsCognitoIdentityUnlinkDeveloperIdentityOptions(
[property: CliOption("--identity-id")] string IdentityId,
[property: CliOption("--identity-pool-id")] string IdentityPoolId,
[property: CliOption("--developer-provider-name")] string DeveloperProviderName,
[property: CliOption("--developer-user-identifier")] string DeveloperUserIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}