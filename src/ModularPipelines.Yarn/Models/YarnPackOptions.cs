using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliSubCommand("pack")]
public record YarnPackOptions : YarnOptions
{
    [CliFlag("--install-if-needed")]
    public virtual bool? InstallIfNeeded { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliOption("--out")]
    public virtual string? Out { get; set; }
}