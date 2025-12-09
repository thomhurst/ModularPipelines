using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CliCommand("set", "version", "from", "sources")]
public record YarnSetVersionFromSourcesOptions : YarnOptions
{
    [CliOption("--path")]
    public virtual string? Path { get; set; }

    [CliOption("--repository")]
    public virtual string? Repository { get; set; }

    [CliOption("--branch")]
    public virtual string? Branch { get; set; }

    [CliOption("--plugin")]
    public virtual string? Plugin { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("--no-minify")]
    public virtual bool? NoMinify { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--skip-plugins")]
    public virtual bool? SkipPlugins { get; set; }
}