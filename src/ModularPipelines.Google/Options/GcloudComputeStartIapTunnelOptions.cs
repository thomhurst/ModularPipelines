using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "start-iap-tunnel")]
public record GcloudComputeStartIapTunnelOptions(
[property: CliArgument] string InstanceName,
[property: CliArgument] string InstancePort
) : GcloudOptions
{
    [CliFlag("--iap-tunnel-disable-connection-check")]
    public bool? IapTunnelDisableConnectionCheck { get; set; }

    [CliOption("--local-host-port")]
    public string? LocalHostPort { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--dest-group")]
    public string? DestGroup { get; set; }
}