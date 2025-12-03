using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "describe-managed-products-by-vendor")]
public record AwsWafv2DescribeManagedProductsByVendorOptions(
[property: CliOption("--vendor-name")] string VendorName,
[property: CliOption("--scope")] string Scope
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}