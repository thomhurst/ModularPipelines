using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerServicePsOptions : DockerOptions
{
    public DockerServicePsOptions(
        IEnumerable<string> service
    )
    {
        CommandParts = ["service", "ps"];

        Service = service;
    }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public IEnumerable<string>? Service { get; set; }

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
