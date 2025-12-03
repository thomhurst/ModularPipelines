using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-http-proxies", "update")]
public record GcloudComputeTargetHttpProxiesUpdateOptions(
[property: CliArgument] string Name,
[property: CliOption("--url-map")] string UrlMap
) : GcloudOptions
{
    [CliFlag("--clear-http-keep-alive-timeout-sec")]
    public bool? ClearHttpKeepAliveTimeoutSec { get; set; }

    [CliOption("--http-keep-alive-timeout-sec")]
    public string? HttpKeepAliveTimeoutSec { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliFlag("--global-url-map")]
    public bool? GlobalUrlMap { get; set; }

    [CliOption("--url-map-region")]
    public string? UrlMapRegion { get; set; }
}