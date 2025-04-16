using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerSearchOptions : DockerOptions
{
    public DockerSearchOptions(
        string term
    )
    {
        CommandParts = ["search"];

        Term = term;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? Term { get; set; }

    [CommandSwitch("--filter")]
    public virtual string? Filter { get; set; }

    [CommandSwitch("--format")]
    public virtual string? Format { get; set; }

    [CommandSwitch("--limit")]
    public virtual string? Limit { get; set; }

    [BooleanCommandSwitch("--no-trunc")]
    public virtual bool? NoTrunc { get; set; }
}
