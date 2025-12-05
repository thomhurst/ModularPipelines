using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "assign-tape-pool")]
public record AwsStoragegatewayAssignTapePoolOptions(
[property: CliOption("--tape-arn")] string TapeArn,
[property: CliOption("--pool-id")] string PoolId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}