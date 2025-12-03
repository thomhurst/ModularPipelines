using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("marketplace-entitlement", "get-entitlements")]
public record AwsMarketplaceEntitlementGetEntitlementsOptions(
[property: CliOption("--product-code")] string ProductCode
) : AwsOptions
{
    [CliOption("--filter")]
    public IEnumerable<KeyValue>? Filter { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}