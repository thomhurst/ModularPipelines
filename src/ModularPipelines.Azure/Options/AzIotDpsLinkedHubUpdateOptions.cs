using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "dps", "linked-hub", "update")]
public record AzIotDpsLinkedHubUpdateOptions(
[property: CommandSwitch("--dps-name")] string DpsName,
[property: CommandSwitch("--linked-hub")] string LinkedHub
) : AzOptions
{
    [CommandSwitch("--allocation-weight")]
    public string? AllocationWeight { get; set; }

    [BooleanCommandSwitch("--apply-allocation-policy")]
    public bool? ApplyAllocationPolicy { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}