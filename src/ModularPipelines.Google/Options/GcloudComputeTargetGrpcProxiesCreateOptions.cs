using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-grpc-proxies", "create")]
public record GcloudComputeTargetGrpcProxiesCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--url-map")] string UrlMap
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--validate-for-proxyless")]
    public bool? ValidateForProxyless { get; set; }

    [BooleanCommandSwitch("--global-url-map")]
    public bool? GlobalUrlMap { get; set; }

    [CommandSwitch("--url-map-region")]
    public string? UrlMapRegion { get; set; }
}