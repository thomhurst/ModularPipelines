using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("plugin inspect")]
[ExcludeFromCodeCoverage]
public record DockerPluginInspectOptions([property: PositionalArgument(Position = Position.AfterArguments)] IEnumerable<string> Plugins) : DockerOptions
{
    [CommandSwitch("--format")]
    public string? Format { get; set; }
}
