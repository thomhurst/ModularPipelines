using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context create")]
[ExcludeFromCodeCoverage]
public record DockerContextCreateOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Context) : DockerOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--docker")]
    public string? Docker { get; set; }

    [CommandSwitch("--from")]
    public string? From { get; set; }
}
