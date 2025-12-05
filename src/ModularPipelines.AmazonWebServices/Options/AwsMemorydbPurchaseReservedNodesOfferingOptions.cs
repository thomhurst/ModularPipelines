using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memorydb", "purchase-reserved-nodes-offering")]
public record AwsMemorydbPurchaseReservedNodesOfferingOptions(
[property: CliOption("--reserved-nodes-offering-id")] string ReservedNodesOfferingId
) : AwsOptions
{
    [CliOption("--reservation-id")]
    public string? ReservationId { get; set; }

    [CliOption("--node-count")]
    public int? NodeCount { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}