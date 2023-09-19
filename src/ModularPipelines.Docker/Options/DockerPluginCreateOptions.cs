using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin create")]
[ExcludeFromCodeCoverage]
public record DockerPluginCreateOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Plugin, [property: PositionalArgument(Position = Position.AfterSwitches)] string PluginDataDirectory) : DockerOptions
{
    [BooleanCommandSwitch("--compress")]
    public bool? Compress { get; set; }
}
