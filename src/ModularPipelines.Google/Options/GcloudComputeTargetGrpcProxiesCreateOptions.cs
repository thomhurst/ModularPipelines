using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-grpc-proxies", "create")]
public record GcloudComputeTargetGrpcProxiesCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--url-map")] string UrlMap
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--validate-for-proxyless")]
    public bool? ValidateForProxyless { get; set; }

    [CliFlag("--global-url-map")]
    public bool? GlobalUrlMap { get; set; }

    [CliOption("--url-map-region")]
    public string? UrlMapRegion { get; set; }
}