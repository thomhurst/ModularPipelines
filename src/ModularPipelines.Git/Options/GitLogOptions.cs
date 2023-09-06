using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("log")]
[ExcludeFromCodeCoverage]
public record GitLogOptions : GitOptions
{
    [BooleanCommandSwitch("--follow")]
    public bool? Follow { get; set; }

    [BooleanCommandSwitch("--no-decorate")]
    public bool? NoDecorate { get; set; }

    [BooleanCommandSwitch("--decorate")]
    public bool? Decorate { get; set; }

    [CommandEqualsSeparatorSwitch("--decorate-refs")]
    public string? DecorateRefs { get; set; }

    [CommandEqualsSeparatorSwitch("--decorate-refs-exclude")]
    public string? DecorateRefsExclude { get; set; }

    [BooleanCommandSwitch("--clear-decorations")]
    public bool? ClearDecorations { get; set; }

    [BooleanCommandSwitch("--source")]
    public bool? Source { get; set; }

    [BooleanCommandSwitch("--no-mailmap")]
    public bool? NoMailmap { get; set; }

    [BooleanCommandSwitch("--mailmap")]
    public bool? Mailmap { get; set; }

    [BooleanCommandSwitch("--no-use-mailmap")]
    public bool? NoUseMailmap { get; set; }

    [BooleanCommandSwitch("--use-mailmap")]
    public bool? UseMailmap { get; set; }

    [BooleanCommandSwitch("--full-diff")]
    public bool? FullDiff { get; set; }

    [BooleanCommandSwitch("--log-size")]
    public bool? LogSize { get; set; }

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

    [CommandEqualsSeparatorSwitch("--committer")]
    public string? Committer { get; set; }

    [CommandEqualsSeparatorSwitch("--grep-reflog")]
    public string? GrepReflog { get; set; }

    [CommandEqualsSeparatorSwitch("--grep")]
    public string? Grep { get; set; }

    [BooleanCommandSwitch("--all-match")]
    public bool? AllMatch { get; set; }

    [BooleanCommandSwitch("--invert-grep")]
    public bool? InvertGrep { get; set; }

    [BooleanCommandSwitch("--regexp-ignore-case")]
    public bool? RegexpIgnoreCase { get; set; }

    [BooleanCommandSwitch("--basic-regexp")]
    public bool? BasicRegexp { get; set; }

    [BooleanCommandSwitch("--extended-regexp")]
    public bool? ExtendedRegexp { get; set; }

    [BooleanCommandSwitch("--fixed-strings")]
    public bool? FixedStrings { get; set; }

    [BooleanCommandSwitch("--perl-regexp")]
    public bool? PerlRegexp { get; set; }

    [BooleanCommandSwitch("--remove-empty")]
    public bool? RemoveEmpty { get; set; }

    [BooleanCommandSwitch("--merges")]
    public bool? Merges { get; set; }

    [BooleanCommandSwitch("--no-merges")]
    public bool? NoMerges { get; set; }

    [CommandEqualsSeparatorSwitch("--min-parents")]
    public string? MinParents { get; set; }

    [CommandEqualsSeparatorSwitch("--max-parents")]
    public string? MaxParents { get; set; }

    [BooleanCommandSwitch("--no-min-parents")]
    public bool? NoMinParents { get; set; }

    [BooleanCommandSwitch("--no-max-parents")]
    public bool? NoMaxParents { get; set; }

    [BooleanCommandSwitch("--first-parent")]
    public bool? FirstParent { get; set; }

    [BooleanCommandSwitch("--exclude-first-parent-only")]
    public bool? ExcludeFirstParentOnly { get; set; }

    [BooleanCommandSwitch("--not")]
    public bool? Not { get; set; }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

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
    public bool? ExcludeHidden { get; set; }

    [BooleanCommandSwitch("--reflog")]
    public bool? Reflog { get; set; }

    [BooleanCommandSwitch("--alternate-refs")]
    public bool? AlternateRefs { get; set; }

    [BooleanCommandSwitch("--single-worktree")]
    public bool? SingleWorktree { get; set; }

    [BooleanCommandSwitch("--ignore-missing")]
    public bool? IgnoreMissing { get; set; }

    [BooleanCommandSwitch("--bisect")]
    public bool? Bisect { get; set; }

    [BooleanCommandSwitch("--stdin")]
    public bool? Stdin { get; set; }

    [BooleanCommandSwitch("--cherry-mark")]
    public bool? CherryMark { get; set; }

    [BooleanCommandSwitch("--cherry-pick")]
    public bool? CherryPick { get; set; }

    [BooleanCommandSwitch("--left-only")]
    public bool? LeftOnly { get; set; }

    [BooleanCommandSwitch("--right-only")]
    public bool? RightOnly { get; set; }

    [BooleanCommandSwitch("--cherry")]
    public bool? Cherry { get; set; }

    [BooleanCommandSwitch("--walk-reflogs")]
    public bool? WalkReflogs { get; set; }

    [BooleanCommandSwitch("--merge")]
    public bool? Merge { get; set; }

    [BooleanCommandSwitch("--boundary")]
    public bool? Boundary { get; set; }

    [BooleanCommandSwitch("--simplify-by-decoration")]
    public bool? SimplifyByDecoration { get; set; }

    [BooleanCommandSwitch("--show-pulls")]
    public bool? ShowPulls { get; set; }

    [BooleanCommandSwitch("--full-history")]
    public bool? FullHistory { get; set; }

    [BooleanCommandSwitch("--dense")]
    public bool? Dense { get; set; }

    [BooleanCommandSwitch("--sparse")]
    public bool? Sparse { get; set; }

    [BooleanCommandSwitch("--simplify-merges")]
    public bool? SimplifyMerges { get; set; }

    [CommandEqualsSeparatorSwitch("--ancestry-path")]
    public string? AncestryPath { get; set; }

    [BooleanCommandSwitch("--date-order")]
    public bool? DateOrder { get; set; }

    [BooleanCommandSwitch("--author-date-order")]
    public bool? AuthorDateOrder { get; set; }

    [BooleanCommandSwitch("--topo-order")]
    public bool? TopoOrder { get; set; }

    [BooleanCommandSwitch("--reverse")]
    public bool? Reverse { get; set; }

    [BooleanCommandSwitch("--no-walk")]
    public bool? NoWalk { get; set; }

    [BooleanCommandSwitch("--do-walk")]
    public bool? DoWalk { get; set; }

    [CommandEqualsSeparatorSwitch("--pretty")]
    public string? Pretty { get; set; }

    [CommandEqualsSeparatorSwitch("--format")]
    public string? Format { get; set; }

    [BooleanCommandSwitch("--abbrev-commit")]
    public bool? AbbrevCommit { get; set; }

    [BooleanCommandSwitch("--no-abbrev-commit")]
    public bool? NoAbbrevCommit { get; set; }

    [BooleanCommandSwitch("--oneline")]
    public bool? Oneline { get; set; }

    [CommandEqualsSeparatorSwitch("--encoding")]
    public string? Encoding { get; set; }

    [CommandEqualsSeparatorSwitch("--expand-tabs")]
    public string? ExpandTabs { get; set; }

    [BooleanCommandSwitch("--no-expand-tabs")]
    public bool? NoExpandTabs { get; set; }

    [CommandEqualsSeparatorSwitch("--notes")]
    public string? Notes { get; set; }

    [BooleanCommandSwitch("--no-notes")]
    public bool? NoNotes { get; set; }

    [CommandEqualsSeparatorSwitch("--show-notes")]
    public string? ShowNotes { get; set; }

    [BooleanCommandSwitch("--no-standard-notes")]
    public bool? NoStandardNotes { get; set; }

    [BooleanCommandSwitch("--standard-notes")]
    public bool? StandardNotes { get; set; }

    [BooleanCommandSwitch("--show-signature")]
    public bool? ShowSignature { get; set; }

    [BooleanCommandSwitch("--relative-date")]
    public bool? RelativeDate { get; set; }

    [CommandEqualsSeparatorSwitch("--date")]
    public string? Date { get; set; }

    [BooleanCommandSwitch("--parents")]
    public bool? Parents { get; set; }

    [BooleanCommandSwitch("--children")]
    public bool? Children { get; set; }

    [BooleanCommandSwitch("--left-right")]
    public bool? LeftRight { get; set; }

    [BooleanCommandSwitch("--graph")]
    public bool? Graph { get; set; }

    [CommandEqualsSeparatorSwitch("--show-linear-break")]
    public string? ShowLinearBreak { get; set; }

    [BooleanCommandSwitch("--patch")]
    public bool? Patch { get; set; }

    [BooleanCommandSwitch("--no-patch")]
    public bool? NoPatch { get; set; }

    [BooleanCommandSwitch("--diff-merges")]
    public bool? DiffMerges { get; set; }

    [BooleanCommandSwitch("--no-diff-merges")]
    public bool? NoDiffMerges { get; set; }

    [BooleanCommandSwitch("--remerge-diff")]
    public bool? RemergeDiff { get; set; }

    [BooleanCommandSwitch("--c")]
    public bool? C { get; set; }

    [BooleanCommandSwitch("--cc")]
    public bool? Cc { get; set; }

    [BooleanCommandSwitch("--combined-all-paths")]
    public bool? CombinedAllPaths { get; set; }

    [CommandEqualsSeparatorSwitch("--unified")]
    public string? Unified { get; set; }

    [CommandEqualsSeparatorSwitch("--output")]
    public string? Output { get; set; }

    [CommandEqualsSeparatorSwitch("--output-indicator-new")]
    public string? OutputIndicatorNew { get; set; }

    [CommandEqualsSeparatorSwitch("--output-indicator-old")]
    public string? OutputIndicatorOld { get; set; }

    [CommandEqualsSeparatorSwitch("--output-indicator-context")]
    public string? OutputIndicatorContext { get; set; }

    [BooleanCommandSwitch("--raw")]
    public bool? Raw { get; set; }

    [BooleanCommandSwitch("--patch-with-raw")]
    public bool? PatchWithRaw { get; set; }

    [BooleanCommandSwitch("--indent-heuristic")]
    public bool? IndentHeuristic { get; set; }

    [BooleanCommandSwitch("--no-indent-heuristic")]
    public bool? NoIndentHeuristic { get; set; }

    [BooleanCommandSwitch("--minimal")]
    public bool? Minimal { get; set; }

    [BooleanCommandSwitch("--patience")]
    public bool? Patience { get; set; }

    [BooleanCommandSwitch("--histogram")]
    public bool? Histogram { get; set; }

    [CommandEqualsSeparatorSwitch("--anchored")]
    public string? Anchored { get; set; }

    [BooleanCommandSwitch("--diff-algorithm")]
    public bool? DiffAlgorithm { get; set; }

    [CommandEqualsSeparatorSwitch("--stat")]
    public string? Stat { get; set; }

    [BooleanCommandSwitch("--compact-summary")]
    public bool? CompactSummary { get; set; }

    [BooleanCommandSwitch("--numstat")]
    public bool? Numstat { get; set; }

    [BooleanCommandSwitch("--shortstat")]
    public bool? Shortstat { get; set; }

    [CommandEqualsSeparatorSwitch("--dirstat")]
    public string? Dirstat { get; set; }

    [BooleanCommandSwitch("--cumulative")]
    public bool? Cumulative { get; set; }

    [CommandEqualsSeparatorSwitch("--dirstat-by-file")]
    public string? DirstatByFile { get; set; }

    [BooleanCommandSwitch("--summary")]
    public bool? Summary { get; set; }

    [BooleanCommandSwitch("--patch-with-stat")]
    public bool? PatchWithStat { get; set; }

    [BooleanCommandSwitch("--name-only")]
    public bool? NameOnly { get; set; }

    [BooleanCommandSwitch("--name-status")]
    public bool? NameStatus { get; set; }

    [CommandEqualsSeparatorSwitch("--submodule")]
    public string? Submodule { get; set; }

    [CommandEqualsSeparatorSwitch("--color")]
    public string? Color { get; set; }

    [BooleanCommandSwitch("--no-color")]
    public bool? NoColor { get; set; }

    [CommandEqualsSeparatorSwitch("--color-moved")]
    public string? ColorMoved { get; set; }

    [BooleanCommandSwitch("--no-color-moved")]
    public bool? NoColorMoved { get; set; }

    [CommandEqualsSeparatorSwitch("--color-moved-ws")]
    public string? ColorMovedWs { get; set; }

    [BooleanCommandSwitch("--no-color-moved-ws")]
    public bool? NoColorMovedWs { get; set; }

    [CommandEqualsSeparatorSwitch("--word-diff")]
    public string? WordDiff { get; set; }

    [CommandEqualsSeparatorSwitch("--word-diff-regex")]
    public string? WordDiffRegex { get; set; }

    [CommandEqualsSeparatorSwitch("--color-words")]
    public string? ColorWords { get; set; }

    [BooleanCommandSwitch("--no-renames")]
    public bool? NoRenames { get; set; }

    [BooleanCommandSwitch("--no-rename-empty")]
    public bool? NoRenameEmpty { get; set; }

    [BooleanCommandSwitch("--rename-empty")]
    public bool? RenameEmpty { get; set; }

    [BooleanCommandSwitch("--check")]
    public bool? Check { get; set; }

    [CommandEqualsSeparatorSwitch("--ws-error-highlight")]
    public string? WsErrorHighlight { get; set; }

    [BooleanCommandSwitch("--full-index")]
    public bool? FullIndex { get; set; }

    [BooleanCommandSwitch("--binary")]
    public bool? Binary { get; set; }

    [CommandEqualsSeparatorSwitch("--abbrev")]
    public string? Abbrev { get; set; }

    [CommandEqualsSeparatorSwitch("--break-rewrites")]
    public string? BreakRewrites { get; set; }

    [CommandEqualsSeparatorSwitch("--find-renames")]
    public string? FindRenames { get; set; }

    [CommandEqualsSeparatorSwitch("--find-copies")]
    public string? FindCopies { get; set; }

    [BooleanCommandSwitch("--find-copies-harder")]
    public bool? FindCopiesHarder { get; set; }

    [BooleanCommandSwitch("--irreversible-delete")]
    public bool? IrreversibleDelete { get; set; }

    [BooleanCommandSwitch("--diff-filter")]
    public bool? DiffFilter { get; set; }

    [CommandEqualsSeparatorSwitch("--find-object")]
    public string? FindObject { get; set; }

    [BooleanCommandSwitch("--pickaxe-all")]
    public bool? PickaxeAll { get; set; }

    [BooleanCommandSwitch("--pickaxe-regex")]
    public bool? PickaxeRegex { get; set; }

    [CommandEqualsSeparatorSwitch("--skip-to")]
    public string? SkipTo { get; set; }

    [CommandEqualsSeparatorSwitch("--rotate-to")]
    public string? RotateTo { get; set; }

    [CommandEqualsSeparatorSwitch("--relative")]
    public string? Relative { get; set; }

    [BooleanCommandSwitch("--no-relative")]
    public bool? NoRelative { get; set; }

    [BooleanCommandSwitch("--text")]
    public bool? Text { get; set; }

    [BooleanCommandSwitch("--ignore-cr-at-eol")]
    public bool? IgnoreCrAtEol { get; set; }

    [BooleanCommandSwitch("--ignore-space-at-eol")]
    public bool? IgnoreSpaceAtEol { get; set; }

    [BooleanCommandSwitch("--ignore-space-change")]
    public bool? IgnoreSpaceChange { get; set; }

    [BooleanCommandSwitch("--ignore-all-space")]
    public bool? IgnoreAllSpace { get; set; }

    [BooleanCommandSwitch("--ignore-blank-lines")]
    public bool? IgnoreBlankLines { get; set; }

    [CommandEqualsSeparatorSwitch("--ignore-matching-lines")]
    public string? IgnoreMatchingLines { get; set; }

    [CommandEqualsSeparatorSwitch("--inter-hunk-context")]
    public string? InterHunkContext { get; set; }

    [BooleanCommandSwitch("--function-context")]
    public bool? FunctionContext { get; set; }

    [BooleanCommandSwitch("--ext-diff")]
    public bool? ExtDiff { get; set; }

    [BooleanCommandSwitch("--no-ext-diff")]
    public bool? NoExtDiff { get; set; }

    [BooleanCommandSwitch("--textconv")]
    public bool? Textconv { get; set; }

    [BooleanCommandSwitch("--no-textconv")]
    public bool? NoTextconv { get; set; }

    [CommandEqualsSeparatorSwitch("--ignore-submodules")]
    public string? IgnoreSubmodules { get; set; }

    [CommandEqualsSeparatorSwitch("--src-prefix")]
    public string? SrcPrefix { get; set; }

    [CommandEqualsSeparatorSwitch("--dst-prefix")]
    public string? DstPrefix { get; set; }

    [BooleanCommandSwitch("--no-prefix")]
    public bool? NoPrefix { get; set; }

    [BooleanCommandSwitch("--default-prefix")]
    public bool? DefaultPrefix { get; set; }

    [CommandEqualsSeparatorSwitch("--line-prefix")]
    public string? LinePrefix { get; set; }

    [BooleanCommandSwitch("--ita-invisible-in-index")]
    public bool? ItaInvisibleInIndex { get; set; }
}
