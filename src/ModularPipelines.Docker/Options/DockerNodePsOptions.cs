using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("node", "ps")]
[ExcludeFromCodeCoverage]
public record DockerNodePsOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Node { get; set; }

    [CommandSwitch("--filter")]
    public virtual string? Filter { get; set; }

    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }

    [BooleanCommandSwitch("--no-resolve")]
    public virtual bool? NoResolve { get; set; }

    [BooleanCommandSwitch("--no-trunc")]
    public virtual bool? NoTrunc { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }
}
