using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-sync", "get-bulk-publish-details")]
public record AwsCognitoSyncGetBulkPublishDetailsOptions(
[property: CliOption("--identity-pool-id")] string IdentityPoolId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}