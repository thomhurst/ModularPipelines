using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout stream")]
[ExcludeFromCodeCoverage]
public record DockerScoutStreamOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Stream, [property: PositionalArgument(Position = Position.AfterSwitches)] string Image) : DockerOptions
{
    [CommandSwitch("--app")]
    public string? App { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }
}