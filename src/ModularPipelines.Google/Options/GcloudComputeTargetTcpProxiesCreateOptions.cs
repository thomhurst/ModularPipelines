using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-tcp-proxies", "create")]
public record GcloudComputeTargetTcpProxiesCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--backend-service")] string BackendService
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--[no-]proxy-bind")]
    public string[]? NoProxyBind { get; set; }

    [CliOption("--proxy-header")]
    public string? ProxyHeader { get; set; }

    [CliOption("--backend-service-region")]
    public string? BackendServiceRegion { get; set; }

    [CliFlag("--global-backend-service")]
    public bool? GlobalBackendService { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}