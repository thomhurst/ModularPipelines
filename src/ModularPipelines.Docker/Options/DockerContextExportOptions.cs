using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("context export")]
[ExcludeFromCodeCoverage]
public record DockerContextExportOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Context) : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? File { get; set; }
}
