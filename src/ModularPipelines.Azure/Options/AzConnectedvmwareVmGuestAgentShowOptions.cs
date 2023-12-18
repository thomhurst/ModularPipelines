using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedvmware", "vm", "guest-agent", "show")]
public record AzConnectedvmwareVmGuestAgentShowOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vm-name")] string VmName
) : AzOptions;