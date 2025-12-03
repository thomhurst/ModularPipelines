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

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? Term { get; set; }

    [CliOption("--filter")]
    public virtual string? Filter { get; set; }

    [CliOption("--format")]
    public virtual string? Format { get; set; }

    [CliOption("--limit")]
    public virtual string? Limit { get; set; }

    [CliFlag("--no-trunc")]
    public virtual bool? NoTrunc { get; set; }
}
