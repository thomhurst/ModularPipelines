using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "network", "private-endpoint", "connection", "set")]
public record AzDtNetworkPrivateEndpointConnectionSetOptions(
[property: CommandSwitch("--cn")] string Cn,
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--status")] string Status
) : AzOptions
{
    [CommandSwitch("--actions-required")]
    public string? ActionsRequired { get; set; }

    [CommandSwitch("--desc")]
    public string? Desc { get; set; }

    [CommandSwitch("--group-ids")]
    public string? GroupIds { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}