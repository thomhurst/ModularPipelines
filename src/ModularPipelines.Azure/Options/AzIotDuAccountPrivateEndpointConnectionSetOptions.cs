using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "account", "private-endpoint-connection", "set")]
public record AzIotDuAccountPrivateEndpointConnectionSetOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--cn")] string Cn,
[property: CommandSwitch("--status")] string Status
) : AzOptions
{
    [CommandSwitch("--desc")]
    public string? Desc { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}