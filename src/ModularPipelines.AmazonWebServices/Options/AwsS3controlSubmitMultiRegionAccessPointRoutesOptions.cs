using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "submit-multi-region-access-point-routes")]
public record AwsS3controlSubmitMultiRegionAccessPointRoutesOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--mrap")] string Mrap,
[property: CliOption("--route-updates")] string[] RouteUpdates
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}