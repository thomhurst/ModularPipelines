using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scvmm", "vm", "guest-agent", "enable")]
public record AzScvmmVmGuestAgentEnableOptions(
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