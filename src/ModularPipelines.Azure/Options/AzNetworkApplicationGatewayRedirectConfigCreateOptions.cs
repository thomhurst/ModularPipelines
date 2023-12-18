using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "redirect-config", "create")]
public record AzNetworkApplicationGatewayRedirectConfigCreateOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [BooleanCommandSwitch("--include-path")]
    public bool? IncludePath { get; set; }

    [BooleanCommandSwitch("--include-query-string")]
    public bool? IncludeQueryString { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--target-listener")]
    public string? TargetListener { get; set; }

    [CommandSwitch("--target-url")]
    public string? TargetUrl { get; set; }
}