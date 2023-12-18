using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedvmware", "vm", "guest-agent", "enable")]
public record AzConnectedvmwareVmGuestAgentEnableOptions(
[property: CommandSwitch("--password")] string Password,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--username")] string Username,
[property: CommandSwitch("--vm-name")] string VmName
) : AzOptions
{
    [CommandSwitch("--https-proxy")]
    public string? HttpsProxy { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}