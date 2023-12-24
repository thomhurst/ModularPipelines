using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearch", "purchase-reserved-instance-offering")]
public record AwsOpensearchPurchaseReservedInstanceOfferingOptions(
[property: CommandSwitch("--reserved-instance-offering-id")] string ReservedInstanceOfferingId,
[property: CommandSwitch("--reservation-name")] string ReservationName
) : AwsOptions
{
    [CommandSwitch("--instance-count")]
    public int? InstanceCount { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}