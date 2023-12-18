using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "bastion", "tunnel")]
public record AzNetworkBastionTunnelOptions(
[property: CommandSwitch("--port")] int Port,
[property: CommandSwitch("--resource-port")] string ResourcePort
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--target-ip-address")]
    public string? TargetIpAddress { get; set; }

    [CommandSwitch("--target-resource-id")]
    public string? TargetResourceId { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}

