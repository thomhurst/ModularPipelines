using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elbv2", "describe-trust-stores")]
public record AwsElbv2DescribeTrustStoresOptions : AwsOptions
{
    [CliOption("--trust-store-arns")]
    public string[]? TrustStoreArns { get; set; }

    [CliOption("--names")]
    public string[]? Names { get; set; }

    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}