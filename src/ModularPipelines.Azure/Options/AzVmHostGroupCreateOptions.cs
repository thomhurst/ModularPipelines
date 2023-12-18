using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "host", "group", "create")]
public record AzVmHostGroupCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--platform-fault-domain-count")] int PlatformFaultDomainCount,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--automatic-placement")]
    public bool? AutomaticPlacement { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--ultra-ssd-enabled")]
    public bool? UltraSsdEnabled { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}

