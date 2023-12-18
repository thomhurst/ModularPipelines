using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "dps", "linked-hub", "create")]
public record AzIotDpsLinkedHubCreateOptions(
[property: CommandSwitch("--dps-name")] string DpsName
) : AzOptions
{
    [CommandSwitch("--allocation-weight")]
    public string? AllocationWeight { get; set; }

    [BooleanCommandSwitch("--apply-allocation-policy")]
    public bool? ApplyAllocationPolicy { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--hn")]
    public string? Hn { get; set; }

    [CommandSwitch("--hrg")]
    public string? Hrg { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

