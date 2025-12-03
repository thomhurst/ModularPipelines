using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-tcp-proxies", "update")]
public record GcloudComputeTargetTcpProxiesUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--backend-service")]
    public string? BackendService { get; set; }

    [CliOption("--proxy-header")]
    public string? ProxyHeader { get; set; }
}