using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-sync", "describe-dataset")]
public record AwsCognitoSyncDescribeDatasetOptions(
[property: CliOption("--identity-pool-id")] string IdentityPoolId,
[property: CliOption("--identity-id")] string IdentityId,
[property: CliOption("--dataset-name")] string DatasetName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}