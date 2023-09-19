using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("stack ps")]
[ExcludeFromCodeCoverage]
public record DockerStackPsOptions([property: PositionalArgument(Position = Position.AfterSwitches)] string Stack) : DockerOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [BooleanCommandSwitch("--no-resolve")]
    public bool? NoResolve { get; set; }

    [BooleanCommandSwitch("--no-trunc")]
    public bool? NoTrunc { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }
}
