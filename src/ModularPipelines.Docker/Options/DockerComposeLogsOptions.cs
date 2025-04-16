using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("compose", "logs")]
[ExcludeFromCodeCoverage]
public record DockerComposeLogsOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Service { get; set; }

    [BooleanCommandSwitch("--follow")]
    public virtual bool? Follow { get; set; }

    [CommandSwitch("--index")]
    public virtual string? Index { get; set; }

    [BooleanCommandSwitch("--no-color")]
    public virtual bool? NoColor { get; set; }

    [CommandSwitch("--no-log-prefix")]
    public virtual string? NoLogPrefix { get; set; }

    [CommandSwitch("--since")]
    public virtual string? Since { get; set; }

    [CommandSwitch("--tail")]
    public virtual string? Tail { get; set; }

    [BooleanCommandSwitch("--timestamps")]
    public virtual bool? Timestamps { get; set; }

    [CommandSwitch("--until")]
    public virtual string? Until { get; set; }
}
