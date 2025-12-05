using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("bin")]
public record YarnBinOptions : YarnOptions
{
    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public virtual string? Name { get; set; }
}