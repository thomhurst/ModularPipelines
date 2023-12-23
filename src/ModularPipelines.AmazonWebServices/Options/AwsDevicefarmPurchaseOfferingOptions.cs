using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devicefarm", "purchase-offering")]
public record AwsDevicefarmPurchaseOfferingOptions(
[property: CommandSwitch("--offering-id")] string OfferingId,
[property: CommandSwitch("--quantity")] int Quantity
) : AwsOptions
{
    [CommandSwitch("--offering-promotion-id")]
    public string? OfferingPromotionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}