using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "put-multi-region-access-point-policy")]
public record AwsS3controlPutMultiRegionAccessPointPolicyOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--details")] string Details
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}