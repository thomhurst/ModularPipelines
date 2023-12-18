using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("capacity", "reservation", "group", "list")]
public record AzCapacityReservationGroupListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--vm-instance")]
    public string? VmInstance { get; set; }

    [CommandSwitch("--vmss-instance")]
    public string? VmssInstance { get; set; }
}