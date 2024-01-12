using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workstations", "start-tcp-tunnel")]
public record GcloudWorkstationsStartTcpTunnelOptions(
[property: PositionalArgument] string Workstation,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Config,
[property: PositionalArgument] string Region,
[property: PositionalArgument] string WorkstationPort
) : GcloudOptions
{
    [CommandSwitch("--local-host-port")]
    public string? LocalHostPort { get; set; }
}