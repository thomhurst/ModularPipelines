using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "probe", "update")]
public record AzNetworkApplicationGatewayProbeUpdateOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [BooleanCommandSwitch("--from-http-settings")]
    public bool? FromHttpSettings { get; set; }

    [BooleanCommandSwitch("--from-settings")]
    public bool? FromSettings { get; set; }

    [CommandSwitch("--host")]
    public string? Host { get; set; }

    [CommandSwitch("--interval")]
    public int? Interval { get; set; }

    [CommandSwitch("--match-body")]
    public string? MatchBody { get; set; }

    [CommandSwitch("--match-status-codes")]
    public string? MatchStatusCodes { get; set; }

    [CommandSwitch("--min-servers")]
    public string? MinServers { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--threshold")]
    public string? Threshold { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}

