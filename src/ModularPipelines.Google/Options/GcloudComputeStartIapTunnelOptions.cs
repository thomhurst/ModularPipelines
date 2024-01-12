using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "start-iap-tunnel")]
public record GcloudComputeStartIapTunnelOptions(
[property: PositionalArgument] string InstanceName,
[property: PositionalArgument] string InstancePort
) : GcloudOptions
{
    [BooleanCommandSwitch("--iap-tunnel-disable-connection-check")]
    public bool? IapTunnelDisableConnectionCheck { get; set; }

    [CommandSwitch("--local-host-port")]
    public string? LocalHostPort { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--dest-group")]
    public string? DestGroup { get; set; }
}