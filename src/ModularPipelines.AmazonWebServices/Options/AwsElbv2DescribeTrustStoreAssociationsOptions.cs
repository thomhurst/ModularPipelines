using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "describe-trust-store-associations")]
public record AwsElbv2DescribeTrustStoreAssociationsOptions(
[property: CliOption("--trust-store-arn")] string TrustStoreArn
) : AwsOptions
{
    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}