using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "vnet-integration", "add")]
public record AzWebappVnetIntegrationAddOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--subnet")] string Subnet,
[property: CommandSwitch("--vnet")] string Vnet
) : AzOptions
{
    [BooleanCommandSwitch("--skip-delegation-check")]
    public bool? SkipDelegationCheck { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }
}