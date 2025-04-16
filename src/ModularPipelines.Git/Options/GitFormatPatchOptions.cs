using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("format-patch")]
[ExcludeFromCodeCoverage]
public record GitFormatPatchOptions : GitOptions
{
    [BooleanCommandSwitch("--no-stat")]
    public virtual bool? NoStat { get; set; }

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

    [BooleanCommandSwitch("--no-renames")]
    public virtual bool? NoRenames { get; set; }

    [BooleanCommandSwitch("--no-rename-empty")]
    public virtual bool? NoRenameEmpty { get; set; }

    [BooleanCommandSwitch("--rename-empty")]
    public virtual bool? RenameEmpty { get; set; }

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

    [CommandEqualsSeparatorSwitch("--output-directory")]
    public string? OutputDirectory { get; set; }

    [BooleanCommandSwitch("--numbered")]
    public virtual bool? Numbered { get; set; }

    [BooleanCommandSwitch("--no-numbered")]
    public virtual bool? NoNumbered { get; set; }

    [CommandEqualsSeparatorSwitch("--start-number")]
    public string? StartNumber { get; set; }

    [BooleanCommandSwitch("--numbered-files")]
    public virtual bool? NumberedFiles { get; set; }

    [BooleanCommandSwitch("--keep-subject")]
    public virtual bool? KeepSubject { get; set; }

    [BooleanCommandSwitch("--signoff")]
    public virtual bool? Signoff { get; set; }

    [BooleanCommandSwitch("--stdout")]
    public virtual bool? Stdout { get; set; }

    [CommandEqualsSeparatorSwitch("--attach")]
    public string? Attach { get; set; }

    [BooleanCommandSwitch("--no-attach")]
    public virtual bool? NoAttach { get; set; }

    [CommandEqualsSeparatorSwitch("--inline")]
    public string? Inline { get; set; }

    [CommandEqualsSeparatorSwitch("--thread")]
    public string? Thread { get; set; }

    [BooleanCommandSwitch("--no-thread")]
    public virtual bool? NoThread { get; set; }

    [CommandEqualsSeparatorSwitch("--in-reply-to")]
    public string? InReplyTo { get; set; }

    [BooleanCommandSwitch("--ignore-if-in-upstream")]
    public virtual bool? IgnoreIfInUpstream { get; set; }

    [BooleanCommandSwitch("--always")]
    public virtual bool? Always { get; set; }

    [CommandEqualsSeparatorSwitch("--cover-from-description")]
    public string? CoverFromDescription { get; set; }

    [CommandEqualsSeparatorSwitch("--subject-prefix")]
    public string? SubjectPrefix { get; set; }

    [CommandEqualsSeparatorSwitch("--filename-max-length")]
    public string? FilenameMaxLength { get; set; }

    [BooleanCommandSwitch("--rfc")]
    public virtual bool? Rfc { get; set; }

    [CommandEqualsSeparatorSwitch("--reroll-count")]
    public string? RerollCount { get; set; }

    [CommandEqualsSeparatorSwitch("--to")]
    public string? To { get; set; }

    [CommandEqualsSeparatorSwitch("--cc")]
    public string? Cc { get; set; }

    [BooleanCommandSwitch("--from")]
    public virtual bool? From { get; set; }

    [BooleanCommandSwitch("--no-force-in-body-from")]
    public virtual bool? NoForceInBodyFrom { get; set; }

    [BooleanCommandSwitch("--force-in-body-from")]
    public virtual bool? ForceInBodyFrom { get; set; }

    [CommandEqualsSeparatorSwitch("--add-header")]
    public string? AddHeader { get; set; }

    [BooleanCommandSwitch("--no-cover-letter")]
    public virtual bool? NoCoverLetter { get; set; }

    [BooleanCommandSwitch("--cover-letter")]
    public virtual bool? CoverLetter { get; set; }

    [BooleanCommandSwitch("--encode-email-headers")]
    public virtual bool? EncodeEmailHeaders { get; set; }

    [BooleanCommandSwitch("--no-encode-email-headers")]
    public virtual bool? NoEncodeEmailHeaders { get; set; }

    [CommandEqualsSeparatorSwitch("--interdiff")]
    public string? Interdiff { get; set; }

    [CommandEqualsSeparatorSwitch("--range-diff")]
    public string? RangeDiff { get; set; }

    [CommandEqualsSeparatorSwitch("--creation-factor")]
    public string? CreationFactor { get; set; }

    [CommandEqualsSeparatorSwitch("--notes")]
    public string? Notes { get; set; }

    [BooleanCommandSwitch("--no-notes")]
    public virtual bool? NoNotes { get; set; }

    [CommandEqualsSeparatorSwitch("--no-signature")]
    public string? NoSignature { get; set; }

    [CommandEqualsSeparatorSwitch("--signature")]
    public string? Signature { get; set; }

    [CommandEqualsSeparatorSwitch("--signature-file")]
    public string? SignatureFile { get; set; }

    [CommandEqualsSeparatorSwitch("--suffix")]
    public string? Suffix { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--no-binary")]
    public virtual bool? NoBinary { get; set; }

    [BooleanCommandSwitch("--zero-commit")]
    public virtual bool? ZeroCommit { get; set; }

    [CommandEqualsSeparatorSwitch("--no-base")]
    public string? NoBase { get; set; }

    [CommandEqualsSeparatorSwitch("--base")]
    public string? Base { get; set; }

    [BooleanCommandSwitch("--root")]
    public virtual bool? Root { get; set; }

    [BooleanCommandSwitch("--progress")]
    public virtual bool? Progress { get; set; }
}