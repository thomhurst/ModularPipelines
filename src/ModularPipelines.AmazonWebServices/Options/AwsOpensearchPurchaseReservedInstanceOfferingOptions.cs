using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearch", "purchase-reserved-instance-offering")]
public record AwsOpensearchPurchaseReservedInstanceOfferingOptions(
[property: CliOption("--reserved-instance-offering-id")] string ReservedInstanceOfferingId,
[property: CliOption("--reservation-name")] string ReservationName
) : AwsOptions
{
    [CliOption("--instance-count")]
    public int? InstanceCount { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}