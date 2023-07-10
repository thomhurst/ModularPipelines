using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("diff-index")]
public record GitDiffIndexOptions : GitOptions
{
    [BooleanCommandSwitch("patch")]
    public bool? Patch { get; set; }

    [BooleanCommandSwitch("no-patch")]
    public bool? NoPatch { get; set; }

    [CommandLongSwitch("unified")]
    public string? Unified { get; set; }

    [CommandLongSwitch("output")]
    public string? Output { get; set; }

    [CommandLongSwitch("output-indicator-new")]
    public string? OutputIndicatorNew { get; set; }

    [CommandLongSwitch("output-indicator-old")]
    public string? OutputIndicatorOld { get; set; }

    [CommandLongSwitch("output-indicator-context")]
    public string? OutputIndicatorContext { get; set; }

    [BooleanCommandSwitch("raw")]
    public bool? Raw { get; set; }

    [BooleanCommandSwitch("patch-with-raw")]
    public bool? PatchWithRaw { get; set; }

    [BooleanCommandSwitch("indent-heuristic")]
    public bool? IndentHeuristic { get; set; }

    [BooleanCommandSwitch("no-indent-heuristic")]
    public bool? NoIndentHeuristic { get; set; }

    [BooleanCommandSwitch("minimal")]
    public bool? Minimal { get; set; }

    [BooleanCommandSwitch("patience")]
    public bool? Patience { get; set; }

    [BooleanCommandSwitch("histogram")]
    public bool? Histogram { get; set; }

    [CommandLongSwitch("anchored")]
    public string? Anchored { get; set; }

    [BooleanCommandSwitch("diff-algorithm")]
    public bool? DiffAlgorithm { get; set; }

    [CommandLongSwitch("stat")]
    public string? Stat { get; set; }

    [BooleanCommandSwitch("compact-summary")]
    public bool? CompactSummary { get; set; }

    [BooleanCommandSwitch("numstat")]
    public bool? Numstat { get; set; }

    [BooleanCommandSwitch("shortstat")]
    public bool? Shortstat { get; set; }

    [CommandLongSwitch("dirstat")]
    public string? Dirstat { get; set; }

    [BooleanCommandSwitch("cumulative")]
    public bool? Cumulative { get; set; }

    [CommandLongSwitch("dirstat-by-file")]
    public string? DirstatByFile { get; set; }

    [BooleanCommandSwitch("summary")]
    public bool? Summary { get; set; }

    [BooleanCommandSwitch("patch-with-stat")]
    public bool? PatchWithStat { get; set; }

    [BooleanCommandSwitch("name-only")]
    public bool? NameOnly { get; set; }

    [BooleanCommandSwitch("name-status")]
    public bool? NameStatus { get; set; }

    [CommandLongSwitch("submodule")]
    public string? Submodule { get; set; }

    [CommandLongSwitch("color")]
    public string? Color { get; set; }

    [BooleanCommandSwitch("no-color")]
    public bool? NoColor { get; set; }

    [CommandLongSwitch("color-moved")]
    public string? ColorMoved { get; set; }

    [BooleanCommandSwitch("no-color-moved")]
    public bool? NoColorMoved { get; set; }

    [CommandLongSwitch("color-moved-ws")]
    public string? ColorMovedWs { get; set; }

    [BooleanCommandSwitch("no-color-moved-ws")]
    public bool? NoColorMovedWs { get; set; }

    [CommandLongSwitch("word-diff")]
    public string? WordDiff { get; set; }

    [CommandLongSwitch("word-diff-regex")]
    public string? WordDiffRegex { get; set; }

    [CommandLongSwitch("color-words")]
    public string? ColorWords { get; set; }

    [BooleanCommandSwitch("no-renames")]
    public bool? NoRenames { get; set; }

    [BooleanCommandSwitch("no-rename-empty")]
    public bool? NoRenameEmpty { get; set; }

    [BooleanCommandSwitch("rename-empty")]
    public bool? RenameEmpty { get; set; }

    [BooleanCommandSwitch("check")]
    public bool? Check { get; set; }

    [CommandLongSwitch("ws-error-highlight")]
    public string? WsErrorHighlight { get; set; }

    [BooleanCommandSwitch("full-index")]
    public bool? FullIndex { get; set; }

    [BooleanCommandSwitch("binary")]
    public bool? Binary { get; set; }

    [CommandLongSwitch("abbrev")]
    public string? Abbrev { get; set; }

    [CommandLongSwitch("break-rewrites")]
    public string? BreakRewrites { get; set; }

    [CommandLongSwitch("find-renames")]
    public string? FindRenames { get; set; }

    [CommandLongSwitch("find-copies")]
    public string? FindCopies { get; set; }

    [BooleanCommandSwitch("find-copies-harder")]
    public bool? FindCopiesHarder { get; set; }

    [BooleanCommandSwitch("irreversible-delete")]
    public bool? IrreversibleDelete { get; set; }

    [BooleanCommandSwitch("diff-filter")]
    public bool? DiffFilter { get; set; }

    [CommandLongSwitch("find-object")]
    public string? FindObject { get; set; }

    [BooleanCommandSwitch("pickaxe-all")]
    public bool? PickaxeAll { get; set; }

    [BooleanCommandSwitch("pickaxe-regex")]
    public bool? PickaxeRegex { get; set; }

    [CommandLongSwitch("skip-to")]
    public string? SkipTo { get; set; }

    [CommandLongSwitch("rotate-to")]
    public string? RotateTo { get; set; }

    [CommandLongSwitch("relative")]
    public string? Relative { get; set; }

    [BooleanCommandSwitch("no-relative")]
    public bool? NoRelative { get; set; }

    [BooleanCommandSwitch("text")]
    public bool? Text { get; set; }

    [BooleanCommandSwitch("ignore-cr-at-eol")]
    public bool? IgnoreCrAtEol { get; set; }

    [BooleanCommandSwitch("ignore-space-at-eol")]
    public bool? IgnoreSpaceAtEol { get; set; }

    [BooleanCommandSwitch("ignore-space-change")]
    public bool? IgnoreSpaceChange { get; set; }

    [BooleanCommandSwitch("ignore-all-space")]
    public bool? IgnoreAllSpace { get; set; }

    [BooleanCommandSwitch("ignore-blank-lines")]
    public bool? IgnoreBlankLines { get; set; }

    [CommandLongSwitch("ignore-matching-lines")]
    public string? IgnoreMatchingLines { get; set; }

    [CommandLongSwitch("inter-hunk-context")]
    public string? InterHunkContext { get; set; }

    [BooleanCommandSwitch("function-context")]
    public bool? FunctionContext { get; set; }

    [BooleanCommandSwitch("exit-code")]
    public bool? ExitCode { get; set; }

    [BooleanCommandSwitch("quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("ext-diff")]
    public bool? ExtDiff { get; set; }

    [BooleanCommandSwitch("no-ext-diff")]
    public bool? NoExtDiff { get; set; }

    [BooleanCommandSwitch("textconv")]
    public bool? Textconv { get; set; }

    [BooleanCommandSwitch("no-textconv")]
    public bool? NoTextconv { get; set; }

    [CommandLongSwitch("ignore-submodules")]
    public string? IgnoreSubmodules { get; set; }

    [CommandLongSwitch("src-prefix")]
    public string? SrcPrefix { get; set; }

    [CommandLongSwitch("dst-prefix")]
    public string? DstPrefix { get; set; }

    [BooleanCommandSwitch("no-prefix")]
    public bool? NoPrefix { get; set; }

    [BooleanCommandSwitch("default-prefix")]
    public bool? DefaultPrefix { get; set; }

    [CommandLongSwitch("line-prefix")]
    public string? LinePrefix { get; set; }

    [BooleanCommandSwitch("ita-invisible-in-index")]
    public bool? ItaInvisibleInIndex { get; set; }

    [BooleanCommandSwitch("cached")]
    public bool? Cached { get; set; }

    [BooleanCommandSwitch("merge-base")]
    public bool? MergeBase { get; set; }

}