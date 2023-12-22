using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network", "pcdp", "create")]
public record AzMobileNetworkPcdpCreateOptions(
[property: CommandSwitch("--access-interface")] string AccessInterface,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--pccp-name")] string PccpName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}