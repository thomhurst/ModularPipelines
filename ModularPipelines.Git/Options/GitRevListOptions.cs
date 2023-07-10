using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("rev-list")]
public record GitRevListOptions : GitOptions
{
    [CommandLongSwitch("max-count")]
    public string? MaxCount { get; set; }

    [CommandLongSwitch("skip")]
    public string? Skip { get; set; }

    [CommandLongSwitch("since")]
    public string? Since { get; set; }

    [CommandLongSwitch("after")]
    public string? After { get; set; }

    [CommandLongSwitch("since-as-filter")]
    public string? SinceAsFilter { get; set; }

    [CommandLongSwitch("until")]
    public string? Until { get; set; }

    [CommandLongSwitch("before")]
    public string? Before { get; set; }

    [CommandLongSwitch("max-age")]
    public string? MaxAge { get; set; }

    [CommandLongSwitch("min-age")]
    public string? MinAge { get; set; }

    [CommandLongSwitch("author")]
    public string? Author { get; set; }

    [CommandLongSwitch("committer")]
    public string? Committer { get; set; }

    [CommandLongSwitch("grep-reflog")]
    public string? GrepReflog { get; set; }

    [CommandLongSwitch("grep")]
    public string? Grep { get; set; }

    [BooleanCommandSwitch("all-match")]
    public bool? AllMatch { get; set; }

    [BooleanCommandSwitch("invert-grep")]
    public bool? InvertGrep { get; set; }

    [BooleanCommandSwitch("regexp-ignore-case")]
    public bool? RegexpIgnoreCase { get; set; }

    [BooleanCommandSwitch("basic-regexp")]
    public bool? BasicRegexp { get; set; }

    [BooleanCommandSwitch("extended-regexp")]
    public bool? ExtendedRegexp { get; set; }

    [BooleanCommandSwitch("fixed-strings")]
    public bool? FixedStrings { get; set; }

    [BooleanCommandSwitch("perl-regexp")]
    public bool? PerlRegexp { get; set; }

    [BooleanCommandSwitch("remove-empty")]
    public bool? RemoveEmpty { get; set; }

    [BooleanCommandSwitch("merges")]
    public bool? Merges { get; set; }

    [BooleanCommandSwitch("no-merges")]
    public bool? NoMerges { get; set; }

    [CommandLongSwitch("min-parents")]
    public string? MinParents { get; set; }

    [CommandLongSwitch("max-parents")]
    public string? MaxParents { get; set; }

    [BooleanCommandSwitch("no-min-parents")]
    public bool? NoMinParents { get; set; }

    [BooleanCommandSwitch("no-max-parents")]
    public bool? NoMaxParents { get; set; }

    [BooleanCommandSwitch("first-parent")]
    public bool? FirstParent { get; set; }

    [BooleanCommandSwitch("exclude-first-parent-only")]
    public bool? ExcludeFirstParentOnly { get; set; }

    [BooleanCommandSwitch("not")]
    public bool? Not { get; set; }

    [BooleanCommandSwitch("all")]
    public bool? All { get; set; }

    [CommandLongSwitch("branches")]
    public string? Branches { get; set; }

    [CommandLongSwitch("tags")]
    public string? Tags { get; set; }

    [CommandLongSwitch("remotes")]
    public string? Remotes { get; set; }

    [CommandLongSwitch("glob")]
    public string? Glob { get; set; }

    [CommandLongSwitch("exclude")]
    public string? Exclude { get; set; }

    [BooleanCommandSwitch("exclude-hidden")]
    public bool? ExcludeHidden { get; set; }

    [BooleanCommandSwitch("reflog")]
    public bool? Reflog { get; set; }

    [BooleanCommandSwitch("alternate-refs")]
    public bool? AlternateRefs { get; set; }

    [BooleanCommandSwitch("single-worktree")]
    public bool? SingleWorktree { get; set; }

    [BooleanCommandSwitch("ignore-missing")]
    public bool? IgnoreMissing { get; set; }

    [BooleanCommandSwitch("stdin")]
    public bool? Stdin { get; set; }

    [BooleanCommandSwitch("quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("disk-usage")]
    public bool? DiskUsage { get; set; }

    [BooleanCommandSwitch("cherry-mark")]
    public bool? CherryMark { get; set; }

    [BooleanCommandSwitch("cherry-pick")]
    public bool? CherryPick { get; set; }

    [BooleanCommandSwitch("left-only")]
    public bool? LeftOnly { get; set; }

    [BooleanCommandSwitch("right-only")]
    public bool? RightOnly { get; set; }

    [BooleanCommandSwitch("cherry")]
    public bool? Cherry { get; set; }

    [BooleanCommandSwitch("walk-reflogs")]
    public bool? WalkReflogs { get; set; }

    [BooleanCommandSwitch("merge")]
    public bool? Merge { get; set; }

    [BooleanCommandSwitch("boundary")]
    public bool? Boundary { get; set; }

    [BooleanCommandSwitch("use-bitmap-index")]
    public bool? UseBitmapIndex { get; set; }

    [CommandLongSwitch("progress")]
    public string? Progress { get; set; }

    [BooleanCommandSwitch("simplify-by-decoration")]
    public bool? SimplifyByDecoration { get; set; }

    [BooleanCommandSwitch("show-pulls")]
    public bool? ShowPulls { get; set; }

    [BooleanCommandSwitch("full-history")]
    public bool? FullHistory { get; set; }

    [BooleanCommandSwitch("dense")]
    public bool? Dense { get; set; }

    [BooleanCommandSwitch("sparse")]
    public bool? Sparse { get; set; }

    [BooleanCommandSwitch("simplify-merges")]
    public bool? SimplifyMerges { get; set; }

    [CommandLongSwitch("ancestry-path")]
    public string? AncestryPath { get; set; }

    [BooleanCommandSwitch("bisect")]
    public bool? Bisect { get; set; }

    [BooleanCommandSwitch("bisect-vars")]
    public bool? BisectVars { get; set; }

    [BooleanCommandSwitch("bisect-all")]
    public bool? BisectAll { get; set; }

    [BooleanCommandSwitch("date-order")]
    public bool? DateOrder { get; set; }

    [BooleanCommandSwitch("author-date-order")]
    public bool? AuthorDateOrder { get; set; }

    [BooleanCommandSwitch("topo-order")]
    public bool? TopoOrder { get; set; }

    [BooleanCommandSwitch("reverse")]
    public bool? Reverse { get; set; }

    [BooleanCommandSwitch("objects")]
    public bool? Objects { get; set; }

    [BooleanCommandSwitch("in-commit-order")]
    public bool? InCommitOrder { get; set; }

    [BooleanCommandSwitch("objects-edge")]
    public bool? ObjectsEdge { get; set; }

    [BooleanCommandSwitch("objects-edge-aggressive")]
    public bool? ObjectsEdgeAggressive { get; set; }

    [BooleanCommandSwitch("indexed-objects")]
    public bool? IndexedObjects { get; set; }

    [BooleanCommandSwitch("unpacked")]
    public bool? Unpacked { get; set; }

    [BooleanCommandSwitch("object-names")]
    public bool? ObjectNames { get; set; }

    [BooleanCommandSwitch("no-object-names")]
    public bool? NoObjectNames { get; set; }

    [CommandLongSwitch("filter")]
    public string? Filter { get; set; }

    [BooleanCommandSwitch("no-filter")]
    public bool? NoFilter { get; set; }

    [BooleanCommandSwitch("filter-provided-objects")]
    public bool? FilterProvidedObjects { get; set; }

    [BooleanCommandSwitch("filter-print-omitted")]
    public bool? FilterPrintOmitted { get; set; }

    [CommandLongSwitch("missing")]
    public string? Missing { get; set; }

    [BooleanCommandSwitch("exclude-promisor-objects")]
    public bool? ExcludePromisorObjects { get; set; }

    [BooleanCommandSwitch("no-walk")]
    public bool? NoWalk { get; set; }

    [BooleanCommandSwitch("do-walk")]
    public bool? DoWalk { get; set; }

    [CommandLongSwitch("pretty")]
    public string? Pretty { get; set; }

    [CommandLongSwitch("format")]
    public string? Format { get; set; }

    [BooleanCommandSwitch("abbrev-commit")]
    public bool? AbbrevCommit { get; set; }

    [BooleanCommandSwitch("no-abbrev-commit")]
    public bool? NoAbbrevCommit { get; set; }

    [BooleanCommandSwitch("oneline")]
    public bool? Oneline { get; set; }

    [CommandLongSwitch("encoding")]
    public string? Encoding { get; set; }

    [CommandLongSwitch("expand-tabs")]
    public string? ExpandTabs { get; set; }

    [BooleanCommandSwitch("no-expand-tabs")]
    public bool? NoExpandTabs { get; set; }

    [BooleanCommandSwitch("show-signature")]
    public bool? ShowSignature { get; set; }

    [BooleanCommandSwitch("relative-date")]
    public bool? RelativeDate { get; set; }

    [CommandLongSwitch("date")]
    public string? Date { get; set; }

    [BooleanCommandSwitch("header")]
    public bool? Header { get; set; }

    [BooleanCommandSwitch("no-commit-header")]
    public bool? NoCommitHeader { get; set; }

    [BooleanCommandSwitch("commit-header")]
    public bool? CommitHeader { get; set; }

    [BooleanCommandSwitch("parents")]
    public bool? Parents { get; set; }

    [BooleanCommandSwitch("children")]
    public bool? Children { get; set; }

    [BooleanCommandSwitch("timestamp")]
    public bool? Timestamp { get; set; }

    [BooleanCommandSwitch("left-right")]
    public bool? LeftRight { get; set; }

    [BooleanCommandSwitch("graph")]
    public bool? Graph { get; set; }

    [CommandLongSwitch("show-linear-break")]
    public string? ShowLinearBreak { get; set; }

    [BooleanCommandSwitch("count")]
    public bool? Count { get; set; }

}