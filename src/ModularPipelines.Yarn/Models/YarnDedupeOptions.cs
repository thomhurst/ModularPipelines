using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("dedupe")]
public record YarnDedupeOptions : YarnOptions
{
    [CliOption("--strategy")]
    public virtual string? Strategy { get; set; }

    [CliFlag("--check")]
    public virtual bool? Check { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliOption("--mode")]
    public virtual string? Mode { get; set; }
}