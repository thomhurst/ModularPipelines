using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("blame")]
[ExcludeFromCodeCoverage]
public record GitBlameOptions : GitOptions
{
    [BooleanCommandSwitch("--root")]
    public virtual bool? Root { get; set; }

    [BooleanCommandSwitch("--show-stats")]
    public virtual bool? ShowStats { get; set; }

    [CommandEqualsSeparatorSwitch("--reverse")]
    public string? Reverse { get; set; }

    [BooleanCommandSwitch("--first-parent")]
    public virtual bool? FirstParent { get; set; }

    [BooleanCommandSwitch("--porcelain")]
    public virtual bool? Porcelain { get; set; }

    [BooleanCommandSwitch("--line-porcelain")]
    public virtual bool? LinePorcelain { get; set; }

    [BooleanCommandSwitch("--incremental")]
    public virtual bool? Incremental { get; set; }

    [CommandEqualsSeparatorSwitch("--encoding")]
    public string? Encoding { get; set; }

    [CommandEqualsSeparatorSwitch("--contents")]
    public string? Contents { get; set; }

    [CommandEqualsSeparatorSwitch("--date")]
    public string? Date { get; set; }

    [BooleanCommandSwitch("--no-progress")]
    public virtual bool? NoProgress { get; set; }

    [BooleanCommandSwitch("--progress")]
    public virtual bool? Progress { get; set; }

    [CommandEqualsSeparatorSwitch("--ignore-rev")]
    public string? IgnoreRev { get; set; }

    [CommandEqualsSeparatorSwitch("--ignore-revs-file")]
    public string? IgnoreRevsFile { get; set; }

    [BooleanCommandSwitch("--color-lines")]
    public virtual bool? ColorLines { get; set; }

    [BooleanCommandSwitch("--color-by-age")]
    public virtual bool? ColorByAge { get; set; }

    [BooleanCommandSwitch("--c")]
    public virtual bool? C { get; set; }

    [BooleanCommandSwitch("--score-debug")]
    public virtual bool? ScoreDebug { get; set; }

    [BooleanCommandSwitch("--show-name")]
    public virtual bool? ShowName { get; set; }

    [BooleanCommandSwitch("--show-number")]
    public virtual bool? ShowNumber { get; set; }

    [BooleanCommandSwitch("--show-email")]
    public virtual bool? ShowEmail { get; set; }

    [CommandEqualsSeparatorSwitch("--abbrev")]
    public string? Abbrev { get; set; }
}