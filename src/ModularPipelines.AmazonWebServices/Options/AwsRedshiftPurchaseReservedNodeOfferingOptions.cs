using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "purchase-reserved-node-offering")]
public record AwsRedshiftPurchaseReservedNodeOfferingOptions(
[property: CliOption("--reserved-node-offering-id")] string ReservedNodeOfferingId
) : AwsOptions
{
    [CliOption("--node-count")]
    public int? NodeCount { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}