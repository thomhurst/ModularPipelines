using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-tcp-proxies", "update")]
public record GcloudComputeTargetTcpProxiesUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--backend-service")]
    public string? BackendService { get; set; }

    [CommandSwitch("--proxy-header")]
    public string? ProxyHeader { get; set; }
}