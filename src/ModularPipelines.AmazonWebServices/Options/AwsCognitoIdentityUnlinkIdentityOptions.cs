using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-identity", "unlink-identity")]
public record AwsCognitoIdentityUnlinkIdentityOptions(
[property: CliOption("--identity-id")] string IdentityId,
[property: CliOption("--logins")] IEnumerable<KeyValue> Logins,
[property: CliOption("--logins-to-remove")] string[] LoginsToRemove
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}