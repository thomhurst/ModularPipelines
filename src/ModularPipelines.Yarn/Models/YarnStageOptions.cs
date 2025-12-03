using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("stage")]
public record YarnStageOptions : YarnOptions
{
    [CliFlag("--commit")]
    public virtual bool? Commit { get; set; }

    [CliFlag("--reset")]
    public virtual bool? Reset { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }
}