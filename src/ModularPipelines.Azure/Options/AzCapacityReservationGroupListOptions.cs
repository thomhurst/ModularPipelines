using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("capacity", "reservation", "group", "list")]
public record AzCapacityReservationGroupListOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--vm-instance")]
    public string? VmInstance { get; set; }

    [CliOption("--vmss-instance")]
    public string? VmssInstance { get; set; }
}