using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin disable")]
[ExcludeFromCodeCoverage]
public record DockerPluginDisableOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Plugin) : DockerOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}
