using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-http-proxies", "update")]
public record GcloudComputeTargetHttpProxiesUpdateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--url-map")] string UrlMap
) : GcloudOptions
{
    [BooleanCommandSwitch("--clear-http-keep-alive-timeout-sec")]
    public bool? ClearHttpKeepAliveTimeoutSec { get; set; }

    [CommandSwitch("--http-keep-alive-timeout-sec")]
    public string? HttpKeepAliveTimeoutSec { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [BooleanCommandSwitch("--global-url-map")]
    public bool? GlobalUrlMap { get; set; }

    [CommandSwitch("--url-map-region")]
    public string? UrlMapRegion { get; set; }
}