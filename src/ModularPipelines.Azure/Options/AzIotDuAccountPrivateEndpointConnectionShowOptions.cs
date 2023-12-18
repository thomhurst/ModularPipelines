using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "account", "private-endpoint-connection", "show")]
public record AzIotDuAccountPrivateEndpointConnectionShowOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--cn")] string Cn
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}