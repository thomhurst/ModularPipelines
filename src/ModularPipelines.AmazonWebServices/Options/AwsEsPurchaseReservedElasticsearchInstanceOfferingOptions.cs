using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("es", "purchase-reserved-elasticsearch-instance-offering")]
public record AwsEsPurchaseReservedElasticsearchInstanceOfferingOptions(
[property: CommandSwitch("--reserved-elasticsearch-instance-offering-id")] string ReservedElasticsearchInstanceOfferingId,
[property: CommandSwitch("--reservation-name")] string ReservationName
) : AwsOptions
{
    [CommandSwitch("--instance-count")]
    public int? InstanceCount { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}