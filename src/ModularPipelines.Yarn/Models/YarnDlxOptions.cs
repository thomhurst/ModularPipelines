using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("dlx")]
public record YarnDlxOptions(
    [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Command
) : YarnOptions
{
    [CliOption("--package")]
    public virtual string? Package { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}