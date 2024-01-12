using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workstations", "ssh")]
public record GcloudWorkstationsSshOptions(
[property: PositionalArgument] string Workstation,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Config,
[property: PositionalArgument] string Region,
[property: PositionalArgument] string SshArgs
) : GcloudOptions
{
    [CommandSwitch("--command")]
    public string? Command { get; set; }

    [CommandSwitch("--port")]
    public string? Port { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }
}