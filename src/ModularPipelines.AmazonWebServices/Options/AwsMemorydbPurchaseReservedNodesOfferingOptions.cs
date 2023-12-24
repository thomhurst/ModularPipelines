using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("memorydb", "purchase-reserved-nodes-offering")]
public record AwsMemorydbPurchaseReservedNodesOfferingOptions(
[property: CommandSwitch("--reserved-nodes-offering-id")] string ReservedNodesOfferingId
) : AwsOptions
{
    [CommandSwitch("--reservation-id")]
    public string? ReservationId { get; set; }

    [CommandSwitch("--node-count")]
    public int? NodeCount { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}