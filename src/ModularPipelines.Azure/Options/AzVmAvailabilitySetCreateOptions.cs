using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "availability-set", "create")]
public record AzVmAvailabilitySetCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--platform-fault-domain-count")]
    public int? PlatformFaultDomainCount { get; set; }

    [CommandSwitch("--platform-update-domain-count")]
    public int? PlatformUpdateDomainCount { get; set; }

    [CommandSwitch("--ppg")]
    public string? Ppg { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--unmanaged")]
    public bool? Unmanaged { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }
}

