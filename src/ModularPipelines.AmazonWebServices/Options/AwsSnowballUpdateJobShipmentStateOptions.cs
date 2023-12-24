using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("snowball", "update-job-shipment-state")]
public record AwsSnowballUpdateJobShipmentStateOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--shipment-state")] string ShipmentState
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}