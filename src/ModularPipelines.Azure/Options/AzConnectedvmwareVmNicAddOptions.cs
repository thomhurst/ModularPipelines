using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedvmware", "vm", "nic", "add")]
public record AzConnectedvmwareVmNicAddOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--network")] string Network,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vm-name")] string VmName
) : AzOptions
{
    [CommandSwitch("--nic-type")]
    public string? NicType { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--power-on-boot")]
    public string? PowerOnBoot { get; set; }
}

