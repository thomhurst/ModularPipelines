using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[ExcludeFromCodeCoverage]
public record DockerStackPsOptions : DockerOptions
{
    public DockerStackPsOptions(
        string stack
    )
    {
        CommandParts = ["stack", "ps"];

        Stack = stack;
    }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string? Stack { get; set; }

    [CliOption("--filter")]
    public virtual string? Filter { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliFlag("--no-resolve")]
    public virtual bool? NoResolve { get; set; }

    [CliFlag("--no-trunc")]
    public virtual bool? NoTrunc { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}
