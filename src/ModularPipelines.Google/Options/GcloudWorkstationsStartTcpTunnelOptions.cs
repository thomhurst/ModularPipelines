using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workstations", "start-tcp-tunnel")]
public record GcloudWorkstationsStartTcpTunnelOptions(
[property: CliArgument] string Workstation,
[property: CliArgument] string Cluster,
[property: CliArgument] string Config,
[property: CliArgument] string Region,
[property: CliArgument] string WorkstationPort
) : GcloudOptions
{
    [CliOption("--local-host-port")]
    public string? LocalHostPort { get; set; }
}