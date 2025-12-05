using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "delete-multi-region-access-point")]
public record AwsS3controlDeleteMultiRegionAccessPointOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--details")] string Details
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}