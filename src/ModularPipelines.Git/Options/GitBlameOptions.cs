using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("blame")]
[ExcludeFromCodeCoverage]
public record GitBlameOptions : GitOptions
{
    [CliFlag("--root")]
    public virtual bool? Root { get; set; }

    [CliFlag("--show-stats")]
    public virtual bool? ShowStats { get; set; }

    [CliOption("--reverse", Format = OptionFormat.EqualsSeparated)]
    public string? Reverse { get; set; }

    [CliFlag("--first-parent")]
    public virtual bool? FirstParent { get; set; }

    [CliFlag("--porcelain")]
    public virtual bool? Porcelain { get; set; }

    [CliFlag("--line-porcelain")]
    public virtual bool? LinePorcelain { get; set; }

    [CliFlag("--incremental")]
    public virtual bool? Incremental { get; set; }

    [CliOption("--encoding", Format = OptionFormat.EqualsSeparated)]
    public string? Encoding { get; set; }

    [CliOption("--contents", Format = OptionFormat.EqualsSeparated)]
    public string? Contents { get; set; }

    [CliOption("--date", Format = OptionFormat.EqualsSeparated)]
    public string? Date { get; set; }

    [CliFlag("--no-progress")]
    public virtual bool? NoProgress { get; set; }

    [CliFlag("--progress")]
    public virtual bool? Progress { get; set; }

    [CliOption("--ignore-rev", Format = OptionFormat.EqualsSeparated)]
    public string? IgnoreRev { get; set; }

    [CliOption("--ignore-revs-file", Format = OptionFormat.EqualsSeparated)]
    public string? IgnoreRevsFile { get; set; }

    [CliFlag("--color-lines")]
    public virtual bool? ColorLines { get; set; }

    [CliFlag("--color-by-age")]
    public virtual bool? ColorByAge { get; set; }

    [CliFlag("--c")]
    public virtual bool? C { get; set; }

    [CliFlag("--score-debug")]
    public virtual bool? ScoreDebug { get; set; }

    [CliFlag("--show-name")]
    public virtual bool? ShowName { get; set; }

    [CliFlag("--show-number")]
    public virtual bool? ShowNumber { get; set; }

    [CliFlag("--show-email")]
    public virtual bool? ShowEmail { get; set; }

    [CliOption("--abbrev", Format = OptionFormat.EqualsSeparated)]
    public string? Abbrev { get; set; }
}