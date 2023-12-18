using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "network", "private-endpoint", "connection", "delete")]
public record AzDtNetworkPrivateEndpointConnectionDeleteOptions(
[property: CommandSwitch("--cn")] string Cn,
[property: CommandSwitch("--dt-name")] string DtName
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}