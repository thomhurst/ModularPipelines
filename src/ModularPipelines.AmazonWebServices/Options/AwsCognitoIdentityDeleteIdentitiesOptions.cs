using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-identity", "delete-identities")]
public record AwsCognitoIdentityDeleteIdentitiesOptions(
[property: CliOption("--identity-ids-to-delete")] string[] IdentityIdsToDelete
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}