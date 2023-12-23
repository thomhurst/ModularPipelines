using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("marketplace-entitlement", "get-entitlements")]
public record AwsMarketplaceEntitlementGetEntitlementsOptions(
[property: CommandSwitch("--product-code")] string ProductCode
) : AwsOptions
{
    [CommandSwitch("--filter")]
    public IEnumerable<KeyValue>? Filter { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}