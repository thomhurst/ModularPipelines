using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("shortlog")]
public record GitShortlogOptions : GitOptions
{
    [BooleanCommandSwitch("numbered")]
    public bool? Numbered { get; set; }

    [BooleanCommandSwitch("summary")]
    public bool? Summary { get; set; }

    [BooleanCommandSwitch("email")]
    public bool? Email { get; set; }

    [CommandLongSwitch("format")]
    public string? Format { get; set; }

    [CommandLongSwitch("date")]
    public string? Date { get; set; }

    [CommandLongSwitch("group")]
    public string? Group { get; set; }

    [BooleanCommandSwitch("c")]
    public bool? C { get; set; }

    [BooleanCommandSwitch("committer")]
    public bool? Committer { get; set; }

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

    [CommandLongSwitch("author")]
    public string? Author { get; set; }

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

    [BooleanCommandSwitch("bisect")]
    public bool? Bisect { get; set; }

    [BooleanCommandSwitch("stdin")]
    public bool? Stdin { get; set; }

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

}