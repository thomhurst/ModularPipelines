using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-tcp-proxies", "create")]
public record GcloudComputeTargetTcpProxiesCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--backend-service")] string BackendService
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--[no-]proxy-bind")]
    public string[]? NoProxyBind { get; set; }

    [CommandSwitch("--proxy-header")]
    public string? ProxyHeader { get; set; }

    [CommandSwitch("--backend-service-region")]
    public string? BackendServiceRegion { get; set; }

    [BooleanCommandSwitch("--global-backend-service")]
    public bool? GlobalBackendService { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}