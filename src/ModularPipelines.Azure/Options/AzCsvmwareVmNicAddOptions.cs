using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("csvmware", "vm", "nic", "add")]
public record AzCsvmwareVmNicAddOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--virtual-network")] string VirtualNetwork,
[property: CommandSwitch("--vm-name")] string VmName
) : AzOptions
{
    [CommandSwitch("--adapter")]
    public string? Adapter { get; set; }

    [BooleanCommandSwitch("--power-on-boot")]
    public bool? PowerOnBoot { get; set; }
}