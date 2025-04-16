using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("log")]
[ExcludeFromCodeCoverage]
public record GitLogOptions : GitOptions
{
    [BooleanCommandSwitch("--follow")]
    public virtual bool? Follow { get; set; }

    [BooleanCommandSwitch("--no-decorate")]
    public virtual bool? NoDecorate { get; set; }

    [BooleanCommandSwitch("--decorate")]
    public virtual bool? Decorate { get; set; }

    [CommandEqualsSeparatorSwitch("--decorate-refs")]
    public string? DecorateRefs { get; set; }

    [CommandEqualsSeparatorSwitch("--decorate-refs-exclude")]
    public string? DecorateRefsExclude { get; set; }

    [BooleanCommandSwitch("--clear-decorations")]
    public virtual bool? ClearDecorations { get; set; }

    [BooleanCommandSwitch("--source")]
    public virtual bool? Source { get; set; }

    [BooleanCommandSwitch("--no-mailmap")]
    public virtual bool? NoMailmap { get; set; }

    [BooleanCommandSwitch("--mailmap")]
    public virtual bool? Mailmap { get; set; }

    [BooleanCommandSwitch("--no-use-mailmap")]
    public virtual bool? NoUseMailmap { get; set; }

    [BooleanCommandSwitch("--use-mailmap")]
    public virtual bool? UseMailmap { get; set; }

    [BooleanCommandSwitch("--full-diff")]
    public virtual bool? FullDiff { get; set; }

    [BooleanCommandSwitch("--log-size")]
    public virtual bool? LogSize { get; set; }

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

    [BooleanCommandSwitch("--date-order")]
    public virtual bool? DateOrder { get; set; }

    [BooleanCommandSwitch("--author-date-order")]
    public virtual bool? AuthorDateOrder { get; set; }

    [BooleanCommandSwitch("--topo-order")]
    public virtual bool? TopoOrder { get; set; }

    [BooleanCommandSwitch("--reverse")]
    public virtual bool? Reverse { get; set; }

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

    [CommandEqualsSeparatorSwitch("--notes")]
    public string? Notes { get; set; }

    [BooleanCommandSwitch("--no-notes")]
    public virtual bool? NoNotes { get; set; }

    [CommandEqualsSeparatorSwitch("--show-notes")]
    public string? ShowNotes { get; set; }

    [BooleanCommandSwitch("--no-standard-notes")]
    public virtual bool? NoStandardNotes { get; set; }

    [BooleanCommandSwitch("--standard-notes")]
    public virtual bool? StandardNotes { get; set; }

    [BooleanCommandSwitch("--show-signature")]
    public virtual bool? ShowSignature { get; set; }

    [BooleanCommandSwitch("--relative-date")]
    public virtual bool? RelativeDate { get; set; }

    [CommandEqualsSeparatorSwitch("--date")]
    public string? Date { get; set; }

    [BooleanCommandSwitch("--parents")]
    public virtual bool? Parents { get; set; }

    [BooleanCommandSwitch("--children")]
    public virtual bool? Children { get; set; }

    [BooleanCommandSwitch("--left-right")]
    public virtual bool? LeftRight { get; set; }

    [BooleanCommandSwitch("--graph")]
    public virtual bool? Graph { get; set; }

    [CommandEqualsSeparatorSwitch("--show-linear-break")]
    public string? ShowLinearBreak { get; set; }

    [BooleanCommandSwitch("--patch")]
    public virtual bool? Patch { get; set; }

    [BooleanCommandSwitch("--no-patch")]
    public virtual bool? NoPatch { get; set; }

    [BooleanCommandSwitch("--diff-merges")]
    public virtual bool? DiffMerges { get; set; }

    [BooleanCommandSwitch("--no-diff-merges")]
    public virtual bool? NoDiffMerges { get; set; }

    [BooleanCommandSwitch("--remerge-diff")]
    public virtual bool? RemergeDiff { get; set; }

    [BooleanCommandSwitch("--c")]
    public virtual bool? C { get; set; }

    [BooleanCommandSwitch("--cc")]
    public virtual bool? Cc { get; set; }

    [BooleanCommandSwitch("--combined-all-paths")]
    public virtual bool? CombinedAllPaths { get; set; }

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
    public virtual bool? Raw { get; set; }

    [BooleanCommandSwitch("--patch-with-raw")]
    public virtual bool? PatchWithRaw { get; set; }

    [BooleanCommandSwitch("--indent-heuristic")]
    public virtual bool? IndentHeuristic { get; set; }

    [BooleanCommandSwitch("--no-indent-heuristic")]
    public virtual bool? NoIndentHeuristic { get; set; }

    [BooleanCommandSwitch("--minimal")]
    public virtual bool? Minimal { get; set; }

    [BooleanCommandSwitch("--patience")]
    public virtual bool? Patience { get; set; }

    [BooleanCommandSwitch("--histogram")]
    public virtual bool? Histogram { get; set; }

    [CommandEqualsSeparatorSwitch("--anchored")]
    public string? Anchored { get; set; }

    [BooleanCommandSwitch("--diff-algorithm")]
    public virtual bool? DiffAlgorithm { get; set; }

    [CommandEqualsSeparatorSwitch("--stat")]
    public string? Stat { get; set; }

    [BooleanCommandSwitch("--compact-summary")]
    public virtual bool? CompactSummary { get; set; }

    [BooleanCommandSwitch("--numstat")]
    public virtual bool? Numstat { get; set; }

    [BooleanCommandSwitch("--shortstat")]
    public virtual bool? Shortstat { get; set; }

    [CommandEqualsSeparatorSwitch("--dirstat")]
    public string? Dirstat { get; set; }

    [BooleanCommandSwitch("--cumulative")]
    public virtual bool? Cumulative { get; set; }

    [CommandEqualsSeparatorSwitch("--dirstat-by-file")]
    public string? DirstatByFile { get; set; }

    [BooleanCommandSwitch("--summary")]
    public virtual bool? Summary { get; set; }

    [BooleanCommandSwitch("--patch-with-stat")]
    public virtual bool? PatchWithStat { get; set; }

    [BooleanCommandSwitch("--name-only")]
    public virtual bool? NameOnly { get; set; }

    [BooleanCommandSwitch("--name-status")]
    public virtual bool? NameStatus { get; set; }

    [CommandEqualsSeparatorSwitch("--submodule")]
    public string? Submodule { get; set; }

    [CommandEqualsSeparatorSwitch("--color")]
    public string? Color { get; set; }

    [BooleanCommandSwitch("--no-color")]
    public virtual bool? NoColor { get; set; }

    [CommandEqualsSeparatorSwitch("--color-moved")]
    public string? ColorMoved { get; set; }

    [BooleanCommandSwitch("--no-color-moved")]
    public virtual bool? NoColorMoved { get; set; }

    [CommandEqualsSeparatorSwitch("--color-moved-ws")]
    public string? ColorMovedWs { get; set; }

    [BooleanCommandSwitch("--no-color-moved-ws")]
    public virtual bool? NoColorMovedWs { get; set; }

    [CommandEqualsSeparatorSwitch("--word-diff")]
    public string? WordDiff { get; set; }

    [CommandEqualsSeparatorSwitch("--word-diff-regex")]
    public string? WordDiffRegex { get; set; }

    [CommandEqualsSeparatorSwitch("--color-words")]
    public string? ColorWords { get; set; }

    [BooleanCommandSwitch("--no-renames")]
    public virtual bool? NoRenames { get; set; }

    [BooleanCommandSwitch("--no-rename-empty")]
    public virtual bool? NoRenameEmpty { get; set; }

    [BooleanCommandSwitch("--rename-empty")]
    public virtual bool? RenameEmpty { get; set; }

    [BooleanCommandSwitch("--check")]
    public virtual bool? Check { get; set; }

    [CommandEqualsSeparatorSwitch("--ws-error-highlight")]
    public string? WsErrorHighlight { get; set; }

    [BooleanCommandSwitch("--full-index")]
    public virtual bool? FullIndex { get; set; }

    [BooleanCommandSwitch("--binary")]
    public virtual bool? Binary { get; set; }

    [CommandEqualsSeparatorSwitch("--abbrev")]
    public string? Abbrev { get; set; }

    [CommandEqualsSeparatorSwitch("--break-rewrites")]
    public string? BreakRewrites { get; set; }

    [CommandEqualsSeparatorSwitch("--find-renames")]
    public string? FindRenames { get; set; }

    [CommandEqualsSeparatorSwitch("--find-copies")]
    public string? FindCopies { get; set; }

    [BooleanCommandSwitch("--find-copies-harder")]
    public virtual bool? FindCopiesHarder { get; set; }

    [BooleanCommandSwitch("--irreversible-delete")]
    public virtual bool? IrreversibleDelete { get; set; }

    [BooleanCommandSwitch("--diff-filter")]
    public virtual bool? DiffFilter { get; set; }

    [CommandEqualsSeparatorSwitch("--find-object")]
    public string? FindObject { get; set; }

    [BooleanCommandSwitch("--pickaxe-all")]
    public virtual bool? PickaxeAll { get; set; }

    [BooleanCommandSwitch("--pickaxe-regex")]
    public virtual bool? PickaxeRegex { get; set; }

    [CommandEqualsSeparatorSwitch("--skip-to")]
    public string? SkipTo { get; set; }

    [CommandEqualsSeparatorSwitch("--rotate-to")]
    public string? RotateTo { get; set; }

    [CommandEqualsSeparatorSwitch("--relative")]
    public string? Relative { get; set; }

    [BooleanCommandSwitch("--no-relative")]
    public virtual bool? NoRelative { get; set; }

    [BooleanCommandSwitch("--text")]
    public virtual bool? Text { get; set; }

    [BooleanCommandSwitch("--ignore-cr-at-eol")]
    public virtual bool? IgnoreCrAtEol { get; set; }

    [BooleanCommandSwitch("--ignore-space-at-eol")]
    public virtual bool? IgnoreSpaceAtEol { get; set; }

    [BooleanCommandSwitch("--ignore-space-change")]
    public virtual bool? IgnoreSpaceChange { get; set; }

    [BooleanCommandSwitch("--ignore-all-space")]
    public virtual bool? IgnoreAllSpace { get; set; }

    [BooleanCommandSwitch("--ignore-blank-lines")]
    public virtual bool? IgnoreBlankLines { get; set; }

    [CommandEqualsSeparatorSwitch("--ignore-matching-lines")]
    public string? IgnoreMatchingLines { get; set; }

    [CommandEqualsSeparatorSwitch("--inter-hunk-context")]
    public string? InterHunkContext { get; set; }

    [BooleanCommandSwitch("--function-context")]
    public virtual bool? FunctionContext { get; set; }

    [BooleanCommandSwitch("--ext-diff")]
    public virtual bool? ExtDiff { get; set; }

    [BooleanCommandSwitch("--no-ext-diff")]
    public virtual bool? NoExtDiff { get; set; }

    [BooleanCommandSwitch("--textconv")]
    public virtual bool? Textconv { get; set; }

    [BooleanCommandSwitch("--no-textconv")]
    public virtual bool? NoTextconv { get; set; }

    [CommandEqualsSeparatorSwitch("--ignore-submodules")]
    public string? IgnoreSubmodules { get; set; }

    [CommandEqualsSeparatorSwitch("--src-prefix")]
    public string? SrcPrefix { get; set; }

    [CommandEqualsSeparatorSwitch("--dst-prefix")]
    public string? DstPrefix { get; set; }

    [BooleanCommandSwitch("--no-prefix")]
    public virtual bool? NoPrefix { get; set; }

    [BooleanCommandSwitch("--default-prefix")]
    public virtual bool? DefaultPrefix { get; set; }

    [CommandEqualsSeparatorSwitch("--line-prefix")]
    public string? LinePrefix { get; set; }

    [BooleanCommandSwitch("--ita-invisible-in-index")]
    public virtual bool? ItaInvisibleInIndex { get; set; }
}