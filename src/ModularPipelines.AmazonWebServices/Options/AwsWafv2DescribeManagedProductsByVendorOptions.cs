using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wafv2", "describe-managed-products-by-vendor")]
public record AwsWafv2DescribeManagedProductsByVendorOptions(
[property: CommandSwitch("--vendor-name")] string VendorName,
[property: CommandSwitch("--scope")] string Scope
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}