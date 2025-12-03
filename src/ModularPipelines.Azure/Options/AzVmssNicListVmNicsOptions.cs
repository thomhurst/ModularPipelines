using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmss", "nic", "list-vm-nics")]
public record AzVmssNicListVmNicsOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--virtual-machine-scale-set-name")] string VirtualMachineScaleSetName
) : AzOptions;