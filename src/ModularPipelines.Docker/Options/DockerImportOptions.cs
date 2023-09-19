using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("import")]
[ExcludeFromCodeCoverage]
public record DockerImportOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Repository { get; set; }

    [CommandSwitch("--change")]
    public string? Change { get; set; }

    [CommandSwitch("--message")]
    public string? Message { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }
}
