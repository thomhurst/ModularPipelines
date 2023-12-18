using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "account", "private-endpoint-connection", "delete")]
public record AzIotDuAccountPrivateEndpointConnectionDeleteOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--cn")] string Cn
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}

