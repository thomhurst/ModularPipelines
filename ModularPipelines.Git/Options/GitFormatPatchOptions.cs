using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("format-patch")]
public record GitFormatPatchOptions : GitOptions
{
    [BooleanCommandSwitch("--no-stat")]
    public bool? NoStat { get; set; }

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

    [BooleanCommandSwitch("--no-renames")]
    public bool? NoRenames { get; set; }

    [BooleanCommandSwitch("--no-rename-empty")]
    public bool? NoRenameEmpty { get; set; }

    [BooleanCommandSwitch("--rename-empty")]
    public bool? RenameEmpty { get; set; }

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

    [CommandEqualsSeparatorSwitch("--output-directory")]
    public string? OutputDirectory { get; set; }

    [BooleanCommandSwitch("--numbered")]
    public bool? Numbered { get; set; }

    [BooleanCommandSwitch("--no-numbered")]
    public bool? NoNumbered { get; set; }

    [CommandEqualsSeparatorSwitch("--start-number")]
    public string? StartNumber { get; set; }

    [BooleanCommandSwitch("--numbered-files")]
    public bool? NumberedFiles { get; set; }

    [BooleanCommandSwitch("--keep-subject")]
    public bool? KeepSubject { get; set; }

    [BooleanCommandSwitch("--signoff")]
    public bool? Signoff { get; set; }

    [BooleanCommandSwitch("--stdout")]
    public bool? Stdout { get; set; }

    [CommandEqualsSeparatorSwitch("--attach")]
    public string? Attach { get; set; }

    [BooleanCommandSwitch("--no-attach")]
    public bool? NoAttach { get; set; }

    [CommandEqualsSeparatorSwitch("--inline")]
    public string? Inline { get; set; }

    [CommandEqualsSeparatorSwitch("--thread")]
    public string? Thread { get; set; }

    [BooleanCommandSwitch("--no-thread")]
    public bool? NoThread { get; set; }

    [CommandEqualsSeparatorSwitch("--in-reply-to")]
    public string? InReplyTo { get; set; }

    [BooleanCommandSwitch("--ignore-if-in-upstream")]
    public bool? IgnoreIfInUpstream { get; set; }

    [BooleanCommandSwitch("--always")]
    public bool? Always { get; set; }

    [CommandEqualsSeparatorSwitch("--cover-from-description")]
    public string? CoverFromDescription { get; set; }

    [CommandEqualsSeparatorSwitch("--subject-prefix")]
    public string? SubjectPrefix { get; set; }

    [CommandEqualsSeparatorSwitch("--filename-max-length")]
    public string? FilenameMaxLength { get; set; }

    [BooleanCommandSwitch("--rfc")]
    public bool? Rfc { get; set; }

    [CommandEqualsSeparatorSwitch("--reroll-count")]
    public string? RerollCount { get; set; }

    [CommandEqualsSeparatorSwitch("--to")]
    public string? To { get; set; }

    [CommandEqualsSeparatorSwitch("--cc")]
    public string? Cc { get; set; }

    [BooleanCommandSwitch("--from")]
    public bool? From { get; set; }

    [BooleanCommandSwitch("--no-force-in-body-from")]
    public bool? NoForceInBodyFrom { get; set; }

    [BooleanCommandSwitch("--force-in-body-from")]
    public bool? ForceInBodyFrom { get; set; }

    [CommandEqualsSeparatorSwitch("--add-header")]
    public string? AddHeader { get; set; }

    [BooleanCommandSwitch("--no-cover-letter")]
    public bool? NoCoverLetter { get; set; }

    [BooleanCommandSwitch("--cover-letter")]
    public bool? CoverLetter { get; set; }

    [BooleanCommandSwitch("--encode-email-headers")]
    public bool? EncodeEmailHeaders { get; set; }

    [BooleanCommandSwitch("--no-encode-email-headers")]
    public bool? NoEncodeEmailHeaders { get; set; }

    [CommandEqualsSeparatorSwitch("--interdiff")]
    public string? Interdiff { get; set; }

    [CommandEqualsSeparatorSwitch("--range-diff")]
    public string? RangeDiff { get; set; }

    [CommandEqualsSeparatorSwitch("--creation-factor")]
    public string? CreationFactor { get; set; }

    [CommandEqualsSeparatorSwitch("--notes")]
    public string? Notes { get; set; }

    [BooleanCommandSwitch("--no-notes")]
    public bool? NoNotes { get; set; }

    [CommandEqualsSeparatorSwitch("--no-signature")]
    public string? NoSignature { get; set; }

    [CommandEqualsSeparatorSwitch("--signature")]
    public string? Signature { get; set; }

    [CommandEqualsSeparatorSwitch("--signature-file")]
    public string? SignatureFile { get; set; }

    [CommandEqualsSeparatorSwitch("--suffix")]
    public string? Suffix { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--no-binary")]
    public bool? NoBinary { get; set; }

    [BooleanCommandSwitch("--zero-commit")]
    public bool? ZeroCommit { get; set; }

    [CommandEqualsSeparatorSwitch("--no-base")]
    public string? NoBase { get; set; }

    [CommandEqualsSeparatorSwitch("--base")]
    public string? Base { get; set; }

    [BooleanCommandSwitch("--root")]
    public bool? Root { get; set; }

    [BooleanCommandSwitch("--progress")]
    public bool? Progress { get; set; }

}