using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("rev-list")]
[ExcludeFromCodeCoverage]
public record GitRevListOptions : GitOptions
{
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

    [CliOption("--max-age", Format = OptionFormat.EqualsSeparated)]
    public string? MaxAge { get; set; }

    [CliOption("--min-age", Format = OptionFormat.EqualsSeparated)]
    public string? MinAge { get; set; }

    [CliOption("--author", Format = OptionFormat.EqualsSeparated)]
    public string? Author { get; set; }

    [CliOption("--committer", Format = OptionFormat.EqualsSeparated)]
    public string? Committer { get; set; }

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

    [CliFlag("--stdin")]
    public virtual bool? Stdin { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--disk-usage")]
    public virtual bool? DiskUsage { get; set; }

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

    [CliFlag("--use-bitmap-index")]
    public virtual bool? UseBitmapIndex { get; set; }

    [CliOption("--progress", Format = OptionFormat.EqualsSeparated)]
    public string? Progress { get; set; }

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

    [CliFlag("--bisect")]
    public virtual bool? Bisect { get; set; }

    [CliFlag("--bisect-vars")]
    public virtual bool? BisectVars { get; set; }

    [CliFlag("--bisect-all")]
    public virtual bool? BisectAll { get; set; }

    [CliFlag("--date-order")]
    public virtual bool? DateOrder { get; set; }

    [CliFlag("--author-date-order")]
    public virtual bool? AuthorDateOrder { get; set; }

    [CliFlag("--topo-order")]
    public virtual bool? TopoOrder { get; set; }

    [CliFlag("--reverse")]
    public virtual bool? Reverse { get; set; }

    [CliFlag("--objects")]
    public virtual bool? Objects { get; set; }

    [CliFlag("--in-commit-order")]
    public virtual bool? InCommitOrder { get; set; }

    [CliFlag("--objects-edge")]
    public virtual bool? ObjectsEdge { get; set; }

    [CliFlag("--objects-edge-aggressive")]
    public virtual bool? ObjectsEdgeAggressive { get; set; }

    [CliFlag("--indexed-objects")]
    public virtual bool? IndexedObjects { get; set; }

    [CliFlag("--unpacked")]
    public virtual bool? Unpacked { get; set; }

    [CliFlag("--object-names")]
    public virtual bool? ObjectNames { get; set; }

    [CliFlag("--no-object-names")]
    public virtual bool? NoObjectNames { get; set; }

    [CliOption("--filter", Format = OptionFormat.EqualsSeparated)]
    public string? Filter { get; set; }

    [CliFlag("--no-filter")]
    public virtual bool? NoFilter { get; set; }

    [CliFlag("--filter-provided-objects")]
    public virtual bool? FilterProvidedObjects { get; set; }

    [CliFlag("--filter-print-omitted")]
    public virtual bool? FilterPrintOmitted { get; set; }

    [CliOption("--missing", Format = OptionFormat.EqualsSeparated)]
    public string? Missing { get; set; }

    [CliFlag("--exclude-promisor-objects")]
    public virtual bool? ExcludePromisorObjects { get; set; }

    [CliFlag("--no-walk")]
    public virtual bool? NoWalk { get; set; }

    [CliFlag("--do-walk")]
    public virtual bool? DoWalk { get; set; }

    [CliOption("--pretty", Format = OptionFormat.EqualsSeparated)]
    public string? Pretty { get; set; }

    [CliOption("--format", Format = OptionFormat.EqualsSeparated)]
    public string? Format { get; set; }

    [CliFlag("--abbrev-commit")]
    public virtual bool? AbbrevCommit { get; set; }

    [CliFlag("--no-abbrev-commit")]
    public virtual bool? NoAbbrevCommit { get; set; }

    [CliFlag("--oneline")]
    public virtual bool? Oneline { get; set; }

    [CliOption("--encoding", Format = OptionFormat.EqualsSeparated)]
    public string? Encoding { get; set; }

    [CliOption("--expand-tabs", Format = OptionFormat.EqualsSeparated)]
    public string? ExpandTabs { get; set; }

    [CliFlag("--no-expand-tabs")]
    public virtual bool? NoExpandTabs { get; set; }

    [CliFlag("--show-signature")]
    public virtual bool? ShowSignature { get; set; }

    [CliFlag("--relative-date")]
    public virtual bool? RelativeDate { get; set; }

    [CliOption("--date", Format = OptionFormat.EqualsSeparated)]
    public string? Date { get; set; }

    [CliFlag("--header")]
    public virtual bool? Header { get; set; }

    [CliFlag("--no-commit-header")]
    public virtual bool? NoCommitHeader { get; set; }

    [CliFlag("--commit-header")]
    public virtual bool? CommitHeader { get; set; }

    [CliFlag("--parents")]
    public virtual bool? Parents { get; set; }

    [CliFlag("--children")]
    public virtual bool? Children { get; set; }

    [CliFlag("--timestamp")]
    public virtual bool? Timestamp { get; set; }

    [CliFlag("--left-right")]
    public virtual bool? LeftRight { get; set; }

    [CliFlag("--graph")]
    public virtual bool? Graph { get; set; }

    [CliOption("--show-linear-break", Format = OptionFormat.EqualsSeparated)]
    public string? ShowLinearBreak { get; set; }

    [CliFlag("--count")]
    public virtual bool? Count { get; set; }
}