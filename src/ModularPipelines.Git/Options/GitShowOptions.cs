using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("show")]
[ExcludeFromCodeCoverage]
public record GitShowOptions : GitOptions
{
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

    [CliOption("--notes", Format = OptionFormat.EqualsSeparated)]
    public string? Notes { get; set; }

    [CliFlag("--no-notes")]
    public virtual bool? NoNotes { get; set; }

    [CliOption("--show-notes", Format = OptionFormat.EqualsSeparated)]
    public string? ShowNotes { get; set; }

    [CliFlag("--no-standard-notes")]
    public virtual bool? NoStandardNotes { get; set; }

    [CliFlag("--standard-notes")]
    public virtual bool? StandardNotes { get; set; }

    [CliFlag("--show-signature")]
    public virtual bool? ShowSignature { get; set; }

    [CliFlag("--patch")]
    public virtual bool? Patch { get; set; }

    [CliFlag("--no-patch")]
    public virtual bool? NoPatch { get; set; }

    [CliFlag("--diff-merges")]
    public virtual bool? DiffMerges { get; set; }

    [CliFlag("--no-diff-merges")]
    public virtual bool? NoDiffMerges { get; set; }

    [CliFlag("--remerge-diff")]
    public virtual bool? RemergeDiff { get; set; }

    [CliFlag("--c")]
    public virtual bool? C { get; set; }

    [CliFlag("--cc")]
    public virtual bool? Cc { get; set; }

    [CliFlag("--combined-all-paths")]
    public virtual bool? CombinedAllPaths { get; set; }

    [CliOption("--unified", Format = OptionFormat.EqualsSeparated)]
    public string? Unified { get; set; }

    [CliOption("--output", Format = OptionFormat.EqualsSeparated)]
    public string? Output { get; set; }

    [CliOption("--output-indicator-new", Format = OptionFormat.EqualsSeparated)]
    public string? OutputIndicatorNew { get; set; }

    [CliOption("--output-indicator-old", Format = OptionFormat.EqualsSeparated)]
    public string? OutputIndicatorOld { get; set; }

    [CliOption("--output-indicator-context", Format = OptionFormat.EqualsSeparated)]
    public string? OutputIndicatorContext { get; set; }

    [CliFlag("--raw")]
    public virtual bool? Raw { get; set; }

    [CliFlag("--patch-with-raw")]
    public virtual bool? PatchWithRaw { get; set; }

    [CliFlag("--indent-heuristic")]
    public virtual bool? IndentHeuristic { get; set; }

    [CliFlag("--no-indent-heuristic")]
    public virtual bool? NoIndentHeuristic { get; set; }

    [CliFlag("--minimal")]
    public virtual bool? Minimal { get; set; }

    [CliFlag("--patience")]
    public virtual bool? Patience { get; set; }

    [CliFlag("--histogram")]
    public virtual bool? Histogram { get; set; }

    [CliOption("--anchored", Format = OptionFormat.EqualsSeparated)]
    public string? Anchored { get; set; }

    [CliFlag("--diff-algorithm")]
    public virtual bool? DiffAlgorithm { get; set; }

    [CliOption("--stat", Format = OptionFormat.EqualsSeparated)]
    public string? Stat { get; set; }

    [CliFlag("--compact-summary")]
    public virtual bool? CompactSummary { get; set; }

    [CliFlag("--numstat")]
    public virtual bool? Numstat { get; set; }

    [CliFlag("--shortstat")]
    public virtual bool? Shortstat { get; set; }

    [CliOption("--dirstat", Format = OptionFormat.EqualsSeparated)]
    public string? Dirstat { get; set; }

    [CliFlag("--cumulative")]
    public virtual bool? Cumulative { get; set; }

    [CliOption("--dirstat-by-file", Format = OptionFormat.EqualsSeparated)]
    public string? DirstatByFile { get; set; }

    [CliFlag("--summary")]
    public virtual bool? Summary { get; set; }

    [CliFlag("--patch-with-stat")]
    public virtual bool? PatchWithStat { get; set; }

    [CliFlag("--name-only")]
    public virtual bool? NameOnly { get; set; }

    [CliFlag("--name-status")]
    public virtual bool? NameStatus { get; set; }

    [CliOption("--submodule", Format = OptionFormat.EqualsSeparated)]
    public string? Submodule { get; set; }

    [CliOption("--color", Format = OptionFormat.EqualsSeparated)]
    public string? Color { get; set; }

    [CliFlag("--no-color")]
    public virtual bool? NoColor { get; set; }

    [CliOption("--color-moved", Format = OptionFormat.EqualsSeparated)]
    public string? ColorMoved { get; set; }

    [CliFlag("--no-color-moved")]
    public virtual bool? NoColorMoved { get; set; }

    [CliOption("--color-moved-ws", Format = OptionFormat.EqualsSeparated)]
    public string? ColorMovedWs { get; set; }

    [CliFlag("--no-color-moved-ws")]
    public virtual bool? NoColorMovedWs { get; set; }

    [CliOption("--word-diff", Format = OptionFormat.EqualsSeparated)]
    public string? WordDiff { get; set; }

    [CliOption("--word-diff-regex", Format = OptionFormat.EqualsSeparated)]
    public string? WordDiffRegex { get; set; }

    [CliOption("--color-words", Format = OptionFormat.EqualsSeparated)]
    public string? ColorWords { get; set; }

    [CliFlag("--no-renames")]
    public virtual bool? NoRenames { get; set; }

    [CliFlag("--no-rename-empty")]
    public virtual bool? NoRenameEmpty { get; set; }

    [CliFlag("--rename-empty")]
    public virtual bool? RenameEmpty { get; set; }

    [CliFlag("--check")]
    public virtual bool? Check { get; set; }

    [CliOption("--ws-error-highlight", Format = OptionFormat.EqualsSeparated)]
    public string? WsErrorHighlight { get; set; }

    [CliFlag("--full-index")]
    public virtual bool? FullIndex { get; set; }

    [CliFlag("--binary")]
    public virtual bool? Binary { get; set; }

    [CliOption("--abbrev", Format = OptionFormat.EqualsSeparated)]
    public string? Abbrev { get; set; }

    [CliOption("--break-rewrites", Format = OptionFormat.EqualsSeparated)]
    public string? BreakRewrites { get; set; }

    [CliOption("--find-renames", Format = OptionFormat.EqualsSeparated)]
    public string? FindRenames { get; set; }

    [CliOption("--find-copies", Format = OptionFormat.EqualsSeparated)]
    public string? FindCopies { get; set; }

    [CliFlag("--find-copies-harder")]
    public virtual bool? FindCopiesHarder { get; set; }

    [CliFlag("--irreversible-delete")]
    public virtual bool? IrreversibleDelete { get; set; }

    [CliFlag("--diff-filter")]
    public virtual bool? DiffFilter { get; set; }

    [CliOption("--find-object", Format = OptionFormat.EqualsSeparated)]
    public string? FindObject { get; set; }

    [CliFlag("--pickaxe-all")]
    public virtual bool? PickaxeAll { get; set; }

    [CliFlag("--pickaxe-regex")]
    public virtual bool? PickaxeRegex { get; set; }

    [CliOption("--skip-to", Format = OptionFormat.EqualsSeparated)]
    public string? SkipTo { get; set; }

    [CliOption("--rotate-to", Format = OptionFormat.EqualsSeparated)]
    public string? RotateTo { get; set; }

    [CliOption("--relative", Format = OptionFormat.EqualsSeparated)]
    public string? Relative { get; set; }

    [CliFlag("--no-relative")]
    public virtual bool? NoRelative { get; set; }

    [CliFlag("--text")]
    public virtual bool? Text { get; set; }

    [CliFlag("--ignore-cr-at-eol")]
    public virtual bool? IgnoreCrAtEol { get; set; }

    [CliFlag("--ignore-space-at-eol")]
    public virtual bool? IgnoreSpaceAtEol { get; set; }

    [CliFlag("--ignore-space-change")]
    public virtual bool? IgnoreSpaceChange { get; set; }

    [CliFlag("--ignore-all-space")]
    public virtual bool? IgnoreAllSpace { get; set; }

    [CliFlag("--ignore-blank-lines")]
    public virtual bool? IgnoreBlankLines { get; set; }

    [CliOption("--ignore-matching-lines", Format = OptionFormat.EqualsSeparated)]
    public string? IgnoreMatchingLines { get; set; }

    [CliOption("--inter-hunk-context", Format = OptionFormat.EqualsSeparated)]
    public string? InterHunkContext { get; set; }

    [CliFlag("--function-context")]
    public virtual bool? FunctionContext { get; set; }

    [CliFlag("--ext-diff")]
    public virtual bool? ExtDiff { get; set; }

    [CliFlag("--no-ext-diff")]
    public virtual bool? NoExtDiff { get; set; }

    [CliFlag("--textconv")]
    public virtual bool? Textconv { get; set; }

    [CliFlag("--no-textconv")]
    public virtual bool? NoTextconv { get; set; }

    [CliOption("--ignore-submodules", Format = OptionFormat.EqualsSeparated)]
    public string? IgnoreSubmodules { get; set; }

    [CliOption("--src-prefix", Format = OptionFormat.EqualsSeparated)]
    public string? SrcPrefix { get; set; }

    [CliOption("--dst-prefix", Format = OptionFormat.EqualsSeparated)]
    public string? DstPrefix { get; set; }

    [CliFlag("--no-prefix")]
    public virtual bool? NoPrefix { get; set; }

    [CliFlag("--default-prefix")]
    public virtual bool? DefaultPrefix { get; set; }

    [CliOption("--line-prefix", Format = OptionFormat.EqualsSeparated)]
    public string? LinePrefix { get; set; }

    [CliFlag("--ita-invisible-in-index")]
    public virtual bool? ItaInvisibleInIndex { get; set; }
}