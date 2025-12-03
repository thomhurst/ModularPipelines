using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-identity", "get-credentials-for-identity")]
public record AwsCognitoIdentityGetCredentialsForIdentityOptions(
[property: CliOption("--identity-id")] string IdentityId
) : AwsOptions
{
    [CliOption("--logins")]
    public IEnumerable<KeyValue>? Logins { get; set; }

    [CliOption("--custom-role-arn")]
    public string? CustomRoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}