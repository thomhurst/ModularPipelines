using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("shortlog")]
[ExcludeFromCodeCoverage]
public record GitShortlogOptions : GitOptions
{
    [BooleanCommandSwitch("--numbered")]
    public virtual bool? Numbered { get; set; }

    [BooleanCommandSwitch("--summary")]
    public virtual bool? Summary { get; set; }

    [BooleanCommandSwitch("--email")]
    public virtual bool? Email { get; set; }

    [CommandEqualsSeparatorSwitch("--format")]
    public string? Format { get; set; }

    [CommandEqualsSeparatorSwitch("--date")]
    public string? Date { get; set; }

    [CommandEqualsSeparatorSwitch("--group")]
    public string? Group { get; set; }

    [BooleanCommandSwitch("--c")]
    public virtual bool? C { get; set; }

    [BooleanCommandSwitch("--committer")]
    public virtual bool? Committer { get; set; }

    [CommandEqualsSeparatorSwitch("--max-count")]
    public string? MaxCount { get; set; }

    [CommandEqualsSeparatorSwitch("--skip")]
    public string? Skip { get; set; }

    [CommandEqualsSeparatorSwitch("--since")]
    public string? Since { get; set; }

    [CommandEqualsSeparatorSwitch("--after")]
    public string? After { get; set; }

    [CommandEqualsSeparatorSwitch("--since-as-filter")]
    public string? SinceAsFilter { get; set; }

    [CommandEqualsSeparatorSwitch("--until")]
    public string? Until { get; set; }

    [CommandEqualsSeparatorSwitch("--before")]
    public string? Before { get; set; }

    [CommandEqualsSeparatorSwitch("--author")]
    public string? Author { get; set; }

    [CommandEqualsSeparatorSwitch("--grep-reflog")]
    public string? GrepReflog { get; set; }

    [CommandEqualsSeparatorSwitch("--grep")]
    public string? Grep { get; set; }

    [BooleanCommandSwitch("--all-match")]
    public virtual bool? AllMatch { get; set; }

    [BooleanCommandSwitch("--invert-grep")]
    public virtual bool? InvertGrep { get; set; }

    [BooleanCommandSwitch("--regexp-ignore-case")]
    public virtual bool? RegexpIgnoreCase { get; set; }

    [BooleanCommandSwitch("--basic-regexp")]
    public virtual bool? BasicRegexp { get; set; }

    [BooleanCommandSwitch("--extended-regexp")]
    public virtual bool? ExtendedRegexp { get; set; }

    [BooleanCommandSwitch("--fixed-strings")]
    public virtual bool? FixedStrings { get; set; }

    [BooleanCommandSwitch("--perl-regexp")]
    public virtual bool? PerlRegexp { get; set; }

    [BooleanCommandSwitch("--remove-empty")]
    public virtual bool? RemoveEmpty { get; set; }

    [BooleanCommandSwitch("--merges")]
    public virtual bool? Merges { get; set; }

    [BooleanCommandSwitch("--no-merges")]
    public virtual bool? NoMerges { get; set; }

    [CommandEqualsSeparatorSwitch("--min-parents")]
    public string? MinParents { get; set; }

    [CommandEqualsSeparatorSwitch("--max-parents")]
    public string? MaxParents { get; set; }

    [BooleanCommandSwitch("--no-min-parents")]
    public virtual bool? NoMinParents { get; set; }

    [BooleanCommandSwitch("--no-max-parents")]
    public virtual bool? NoMaxParents { get; set; }

    [BooleanCommandSwitch("--first-parent")]
    public virtual bool? FirstParent { get; set; }

    [BooleanCommandSwitch("--exclude-first-parent-only")]
    public virtual bool? ExcludeFirstParentOnly { get; set; }

    [BooleanCommandSwitch("--not")]
    public virtual bool? Not { get; set; }

    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [CommandEqualsSeparatorSwitch("--branches")]
    public string? Branches { get; set; }

    [CommandEqualsSeparatorSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandEqualsSeparatorSwitch("--remotes")]
    public string? Remotes { get; set; }

    [CommandEqualsSeparatorSwitch("--glob")]
    public string? Glob { get; set; }

    [CommandEqualsSeparatorSwitch("--exclude")]
    public string? Exclude { get; set; }

    [BooleanCommandSwitch("--exclude-hidden")]
    public virtual bool? ExcludeHidden { get; set; }

    [BooleanCommandSwitch("--reflog")]
    public virtual bool? Reflog { get; set; }

    [BooleanCommandSwitch("--alternate-refs")]
    public virtual bool? AlternateRefs { get; set; }

    [BooleanCommandSwitch("--single-worktree")]
    public virtual bool? SingleWorktree { get; set; }

    [BooleanCommandSwitch("--ignore-missing")]
    public virtual bool? IgnoreMissing { get; set; }

    [BooleanCommandSwitch("--bisect")]
    public virtual bool? Bisect { get; set; }

    [BooleanCommandSwitch("--stdin")]
    public virtual bool? Stdin { get; set; }

    [BooleanCommandSwitch("--cherry-mark")]
    public virtual bool? CherryMark { get; set; }

    [BooleanCommandSwitch("--cherry-pick")]
    public virtual bool? CherryPick { get; set; }

    [BooleanCommandSwitch("--left-only")]
    public virtual bool? LeftOnly { get; set; }

    [BooleanCommandSwitch("--right-only")]
    public virtual bool? RightOnly { get; set; }

    [BooleanCommandSwitch("--cherry")]
    public virtual bool? Cherry { get; set; }

    [BooleanCommandSwitch("--walk-reflogs")]
    public virtual bool? WalkReflogs { get; set; }

    [BooleanCommandSwitch("--merge")]
    public virtual bool? Merge { get; set; }

    [BooleanCommandSwitch("--boundary")]
    public virtual bool? Boundary { get; set; }

    [BooleanCommandSwitch("--simplify-by-decoration")]
    public virtual bool? SimplifyByDecoration { get; set; }

    [BooleanCommandSwitch("--show-pulls")]
    public virtual bool? ShowPulls { get; set; }

    [BooleanCommandSwitch("--full-history")]
    public virtual bool? FullHistory { get; set; }

    [BooleanCommandSwitch("--dense")]
    public virtual bool? Dense { get; set; }

    [BooleanCommandSwitch("--sparse")]
    public virtual bool? Sparse { get; set; }

    [BooleanCommandSwitch("--simplify-merges")]
    public virtual bool? SimplifyMerges { get; set; }

    [CommandEqualsSeparatorSwitch("--ancestry-path")]
    public string? AncestryPath { get; set; }
}