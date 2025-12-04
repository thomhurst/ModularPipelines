using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("shortlog")]
[ExcludeFromCodeCoverage]
public record GitShortlogOptions : GitOptions
{
    [CliFlag("--numbered")]
    public virtual bool? Numbered { get; set; }

    [CliFlag("--summary")]
    public virtual bool? Summary { get; set; }

    [CliFlag("--email")]
    public virtual bool? Email { get; set; }

    [CliOption("--format", Format = OptionFormat.EqualsSeparated)]
    public string? Format { get; set; }

    [CliOption("--date", Format = OptionFormat.EqualsSeparated)]
    public string? Date { get; set; }

    [CliOption("--group", Format = OptionFormat.EqualsSeparated)]
    public string? Group { get; set; }

    [CliFlag("--c")]
    public virtual bool? C { get; set; }

    [CliFlag("--committer")]
    public virtual bool? Committer { get; set; }

    [CliOption("--max-count", Format = OptionFormat.EqualsSeparated)]
    public string? MaxCount { get; set; }

    [CliOption("--skip", Format = OptionFormat.EqualsSeparated)]
    public string? Skip { get; set; }

    [CliOption("--since", Format = OptionFormat.EqualsSeparated)]
    public string? Since { get; set; }

    [CliOption("--after", Format = OptionFormat.EqualsSeparated)]
    public string? After { get; set; }

    [CliOption("--since-as-filter", Format = OptionFormat.EqualsSeparated)]
    public string? SinceAsFilter { get; set; }

    [CliOption("--until", Format = OptionFormat.EqualsSeparated)]
    public string? Until { get; set; }

    [CliOption("--before", Format = OptionFormat.EqualsSeparated)]
    public string? Before { get; set; }

    [CliOption("--author", Format = OptionFormat.EqualsSeparated)]
    public string? Author { get; set; }

    [CliOption("--grep-reflog", Format = OptionFormat.EqualsSeparated)]
    public string? GrepReflog { get; set; }

    [CliOption("--grep", Format = OptionFormat.EqualsSeparated)]
    public string? Grep { get; set; }

    [CliFlag("--all-match")]
    public virtual bool? AllMatch { get; set; }

    [CliFlag("--invert-grep")]
    public virtual bool? InvertGrep { get; set; }

    [CliFlag("--regexp-ignore-case")]
    public virtual bool? RegexpIgnoreCase { get; set; }

    [CliFlag("--basic-regexp")]
    public virtual bool? BasicRegexp { get; set; }

    [CliFlag("--extended-regexp")]
    public virtual bool? ExtendedRegexp { get; set; }

    [CliFlag("--fixed-strings")]
    public virtual bool? FixedStrings { get; set; }

    [CliFlag("--perl-regexp")]
    public virtual bool? PerlRegexp { get; set; }

    [CliFlag("--remove-empty")]
    public virtual bool? RemoveEmpty { get; set; }

    [CliFlag("--merges")]
    public virtual bool? Merges { get; set; }

    [CliFlag("--no-merges")]
    public virtual bool? NoMerges { get; set; }

    [CliOption("--min-parents", Format = OptionFormat.EqualsSeparated)]
    public string? MinParents { get; set; }

    [CliOption("--max-parents", Format = OptionFormat.EqualsSeparated)]
    public string? MaxParents { get; set; }

    [CliFlag("--no-min-parents")]
    public virtual bool? NoMinParents { get; set; }

    [CliFlag("--no-max-parents")]
    public virtual bool? NoMaxParents { get; set; }

    [CliFlag("--first-parent")]
    public virtual bool? FirstParent { get; set; }

    [CliFlag("--exclude-first-parent-only")]
    public virtual bool? ExcludeFirstParentOnly { get; set; }

    [CliFlag("--not")]
    public virtual bool? Not { get; set; }

    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliOption("--branches", Format = OptionFormat.EqualsSeparated)]
    public string? Branches { get; set; }

    [CliOption("--tags", Format = OptionFormat.EqualsSeparated)]
    public string? Tags { get; set; }

    [CliOption("--remotes", Format = OptionFormat.EqualsSeparated)]
    public string? Remotes { get; set; }

    [CliOption("--glob", Format = OptionFormat.EqualsSeparated)]
    public string? Glob { get; set; }

    [CliOption("--exclude", Format = OptionFormat.EqualsSeparated)]
    public string? Exclude { get; set; }

    [CliFlag("--exclude-hidden")]
    public virtual bool? ExcludeHidden { get; set; }

    [CliFlag("--reflog")]
    public virtual bool? Reflog { get; set; }

    [CliFlag("--alternate-refs")]
    public virtual bool? AlternateRefs { get; set; }

    [CliFlag("--single-worktree")]
    public virtual bool? SingleWorktree { get; set; }

    [CliFlag("--ignore-missing")]
    public virtual bool? IgnoreMissing { get; set; }

    [CliFlag("--bisect")]
    public virtual bool? Bisect { get; set; }

    [CliFlag("--stdin")]
    public virtual bool? Stdin { get; set; }

    [CliFlag("--cherry-mark")]
    public virtual bool? CherryMark { get; set; }

    [CliFlag("--cherry-pick")]
    public virtual bool? CherryPick { get; set; }

    [CliFlag("--left-only")]
    public virtual bool? LeftOnly { get; set; }

    [CliFlag("--right-only")]
    public virtual bool? RightOnly { get; set; }

    [CliFlag("--cherry")]
    public virtual bool? Cherry { get; set; }

    [CliFlag("--walk-reflogs")]
    public virtual bool? WalkReflogs { get; set; }

    [CliFlag("--merge")]
    public virtual bool? Merge { get; set; }

    [CliFlag("--boundary")]
    public virtual bool? Boundary { get; set; }

    [CliFlag("--simplify-by-decoration")]
    public virtual bool? SimplifyByDecoration { get; set; }

    [CliFlag("--show-pulls")]
    public virtual bool? ShowPulls { get; set; }

    [CliFlag("--full-history")]
    public virtual bool? FullHistory { get; set; }

    [CliFlag("--dense")]
    public virtual bool? Dense { get; set; }

    [CliFlag("--sparse")]
    public virtual bool? Sparse { get; set; }

    [CliFlag("--simplify-merges")]
    public virtual bool? SimplifyMerges { get; set; }

    [CliOption("--ancestry-path", Format = OptionFormat.EqualsSeparated)]
    public string? AncestryPath { get; set; }
}