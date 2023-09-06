using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("show")]
[ExcludeFromCodeCoverage]
public record GitShowOptions : GitOptions
{
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
