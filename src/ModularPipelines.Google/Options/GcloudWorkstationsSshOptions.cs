using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workstations", "ssh")]
public record GcloudWorkstationsSshOptions(
[property: CliArgument] string Workstation,
[property: CliArgument] string Cluster,
[property: CliArgument] string Config,
[property: CliArgument] string Region,
[property: CliArgument] string SshArgs
) : GcloudOptions
{
    [CliOption("--command")]
    public string? Command { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }

    [CliOption("--user")]
    public string? User { get; set; }
}