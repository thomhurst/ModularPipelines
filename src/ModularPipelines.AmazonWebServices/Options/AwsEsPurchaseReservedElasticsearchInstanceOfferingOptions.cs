using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("es", "purchase-reserved-elasticsearch-instance-offering")]
public record AwsEsPurchaseReservedElasticsearchInstanceOfferingOptions(
[property: CliOption("--reserved-elasticsearch-instance-offering-id")] string ReservedElasticsearchInstanceOfferingId,
[property: CliOption("--reservation-name")] string ReservationName
) : AwsOptions
{
    [CliOption("--instance-count")]
    public int? InstanceCount { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}