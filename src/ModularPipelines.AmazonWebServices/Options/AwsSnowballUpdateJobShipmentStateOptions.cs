using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("snowball", "update-job-shipment-state")]
public record AwsSnowballUpdateJobShipmentStateOptions(
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--shipment-state")] string ShipmentState
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}