using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin enable")]
[ExcludeFromCodeCoverage]
public record DockerPluginEnableOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Plugin) : DockerOptions
{
    [CommandSwitch("--timeout")]
    public int? Timeout { get; set; }
}