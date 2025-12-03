using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devicefarm", "purchase-offering")]
public record AwsDevicefarmPurchaseOfferingOptions(
[property: CliOption("--offering-id")] string OfferingId,
[property: CliOption("--quantity")] int Quantity
) : AwsOptions
{
    [CliOption("--offering-promotion-id")]
    public string? OfferingPromotionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}