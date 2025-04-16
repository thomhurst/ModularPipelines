using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("rev-list")]
[ExcludeFromCodeCoverage]
public record GitRevListOptions : GitOptions
{
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

    [CommandEqualsSeparatorSwitch("--max-age")]
    public string? MaxAge { get; set; }

    [CommandEqualsSeparatorSwitch("--min-age")]
    public string? MinAge { get; set; }

    [CommandEqualsSeparatorSwitch("--author")]
    public string? Author { get; set; }

    [CommandEqualsSeparatorSwitch("--committer")]
    public string? Committer { get; set; }

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

    [BooleanCommandSwitch("--stdin")]
    public virtual bool? Stdin { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--disk-usage")]
    public virtual bool? DiskUsage { get; set; }

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

    [BooleanCommandSwitch("--use-bitmap-index")]
    public virtual bool? UseBitmapIndex { get; set; }

    [CommandEqualsSeparatorSwitch("--progress")]
    public string? Progress { get; set; }

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

    [BooleanCommandSwitch("--bisect")]
    public virtual bool? Bisect { get; set; }

    [BooleanCommandSwitch("--bisect-vars")]
    public virtual bool? BisectVars { get; set; }

    [BooleanCommandSwitch("--bisect-all")]
    public virtual bool? BisectAll { get; set; }

    [BooleanCommandSwitch("--date-order")]
    public virtual bool? DateOrder { get; set; }

    [BooleanCommandSwitch("--author-date-order")]
    public virtual bool? AuthorDateOrder { get; set; }

    [BooleanCommandSwitch("--topo-order")]
    public virtual bool? TopoOrder { get; set; }

    [BooleanCommandSwitch("--reverse")]
    public virtual bool? Reverse { get; set; }

    [BooleanCommandSwitch("--objects")]
    public virtual bool? Objects { get; set; }

    [BooleanCommandSwitch("--in-commit-order")]
    public virtual bool? InCommitOrder { get; set; }

    [BooleanCommandSwitch("--objects-edge")]
    public virtual bool? ObjectsEdge { get; set; }

    [BooleanCommandSwitch("--objects-edge-aggressive")]
    public virtual bool? ObjectsEdgeAggressive { get; set; }

    [BooleanCommandSwitch("--indexed-objects")]
    public virtual bool? IndexedObjects { get; set; }

    [BooleanCommandSwitch("--unpacked")]
    public virtual bool? Unpacked { get; set; }

    [BooleanCommandSwitch("--object-names")]
    public virtual bool? ObjectNames { get; set; }

    [BooleanCommandSwitch("--no-object-names")]
    public virtual bool? NoObjectNames { get; set; }

    [CommandEqualsSeparatorSwitch("--filter")]
    public string? Filter { get; set; }

    [BooleanCommandSwitch("--no-filter")]
    public virtual bool? NoFilter { get; set; }

    [BooleanCommandSwitch("--filter-provided-objects")]
    public virtual bool? FilterProvidedObjects { get; set; }

    [BooleanCommandSwitch("--filter-print-omitted")]
    public virtual bool? FilterPrintOmitted { get; set; }

    [CommandEqualsSeparatorSwitch("--missing")]
    public string? Missing { get; set; }

    [BooleanCommandSwitch("--exclude-promisor-objects")]
    public virtual bool? ExcludePromisorObjects { get; set; }

    [BooleanCommandSwitch("--no-walk")]
    public virtual bool? NoWalk { get; set; }

    [BooleanCommandSwitch("--do-walk")]
    public virtual bool? DoWalk { get; set; }

    [CommandEqualsSeparatorSwitch("--pretty")]
    public string? Pretty { get; set; }

    [CommandEqualsSeparatorSwitch("--format")]
    public string? Format { get; set; }

    [BooleanCommandSwitch("--abbrev-commit")]
    public virtual bool? AbbrevCommit { get; set; }

    [BooleanCommandSwitch("--no-abbrev-commit")]
    public virtual bool? NoAbbrevCommit { get; set; }

    [BooleanCommandSwitch("--oneline")]
    public virtual bool? Oneline { get; set; }

    [CommandEqualsSeparatorSwitch("--encoding")]
    public string? Encoding { get; set; }

    [CommandEqualsSeparatorSwitch("--expand-tabs")]
    public string? ExpandTabs { get; set; }

    [BooleanCommandSwitch("--no-expand-tabs")]
    public virtual bool? NoExpandTabs { get; set; }

    [BooleanCommandSwitch("--show-signature")]
    public virtual bool? ShowSignature { get; set; }

    [BooleanCommandSwitch("--relative-date")]
    public virtual bool? RelativeDate { get; set; }

    [CommandEqualsSeparatorSwitch("--date")]
    public string? Date { get; set; }

    [BooleanCommandSwitch("--header")]
    public virtual bool? Header { get; set; }

    [BooleanCommandSwitch("--no-commit-header")]
    public virtual bool? NoCommitHeader { get; set; }

    [BooleanCommandSwitch("--commit-header")]
    public virtual bool? CommitHeader { get; set; }

    [BooleanCommandSwitch("--parents")]
    public virtual bool? Parents { get; set; }

    [BooleanCommandSwitch("--children")]
    public virtual bool? Children { get; set; }

    [BooleanCommandSwitch("--timestamp")]
    public virtual bool? Timestamp { get; set; }

    [BooleanCommandSwitch("--left-right")]
    public virtual bool? LeftRight { get; set; }

    [BooleanCommandSwitch("--graph")]
    public virtual bool? Graph { get; set; }

    [CommandEqualsSeparatorSwitch("--show-linear-break")]
    public string? ShowLinearBreak { get; set; }

    [BooleanCommandSwitch("--count")]
    public virtual bool? Count { get; set; }
}