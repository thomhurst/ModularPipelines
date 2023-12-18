using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "redirect-config", "list")]
public record AzNetworkApplicationGatewayRedirectConfigListOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [BooleanCommandSwitch("--include-path")]
    public bool? IncludePath { get; set; }

    [BooleanCommandSwitch("--include-query-string")]
    public bool? IncludeQueryString { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--target-listener")]
    public string? TargetListener { get; set; }

    [CommandSwitch("--target-url")]
    public string? TargetUrl { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}

