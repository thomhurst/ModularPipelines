using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmss", "nic", "list-vm-nics")]
public record AzVmssNicListVmNicsOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--virtual-machine-scale-set-name")] string VirtualMachineScaleSetName
) : AzOptions;