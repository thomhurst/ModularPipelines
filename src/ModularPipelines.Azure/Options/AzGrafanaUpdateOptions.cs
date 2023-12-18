using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "update")]
public record AzGrafanaUpdateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--api-key")]
    public string? ApiKey { get; set; }

    [CommandSwitch("--deterministic-outbound-ip")]
    public string? DeterministicOutboundIp { get; set; }

    [CommandSwitch("--from-address")]
    public string? FromAddress { get; set; }

    [CommandSwitch("--from-name")]
    public string? FromName { get; set; }

    [CommandSwitch("--host")]
    public string? Host { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--skip-verify")]
    public bool? SkipVerify { get; set; }

    [CommandSwitch("--smtp")]
    public string? Smtp { get; set; }

    [CommandSwitch("--start-tls-policy")]
    public string? StartTlsPolicy { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }
}