using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("diff-index")]
[ExcludeFromCodeCoverage]
public record GitDiffIndexOptions : GitOptions
{
    [BooleanCommandSwitch("--patch")]
    public virtual bool? Patch { get; set; }

    [BooleanCommandSwitch("--no-patch")]
    public virtual bool? NoPatch { get; set; }

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

    [BooleanCommandSwitch("--exit-code")]
    public virtual bool? ExitCode { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

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

    [BooleanCommandSwitch("--cached")]
    public virtual bool? Cached { get; set; }

    [BooleanCommandSwitch("--merge-base")]
    public virtual bool? MergeBase { get; set; }
}