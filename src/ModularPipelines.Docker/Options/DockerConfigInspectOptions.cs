using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("config inspect")]
[ExcludeFromCodeCoverage]
public record DockerConfigInspectOptions([property: PositionalArgument(Position = Position.AfterSwitches)] IEnumerable<string> ConfigNames) : DockerOptions
{
    [CommandSwitch("--pretty")]
    public string? Pretty { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }
}