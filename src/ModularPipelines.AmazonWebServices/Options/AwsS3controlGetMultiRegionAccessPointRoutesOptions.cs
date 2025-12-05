using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "get-multi-region-access-point-routes")]
public record AwsS3controlGetMultiRegionAccessPointRoutesOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--mrap")] string Mrap
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}