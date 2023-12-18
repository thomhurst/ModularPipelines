using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedvmware", "vm", "nic", "update")]
public record AzConnectedvmwareVmNicUpdateOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vm-name")] string VmName
) : AzOptions
{
    [CommandSwitch("--device-key")]
    public string? DeviceKey { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--power-on-boot")]
    public string? PowerOnBoot { get; set; }
}