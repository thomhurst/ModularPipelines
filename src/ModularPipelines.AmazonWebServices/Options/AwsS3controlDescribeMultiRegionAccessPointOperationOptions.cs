using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "describe-multi-region-access-point-operation")]
public record AwsS3controlDescribeMultiRegionAccessPointOperationOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--request-token-arn")] string RequestTokenArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}