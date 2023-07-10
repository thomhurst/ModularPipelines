using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("format-patch")]
public record GitFormatPatchOptions : GitOptions
{
    [BooleanCommandSwitch("no-stat")]
    public bool? NoStat { get; set; }

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

    [BooleanCommandSwitch("no-renames")]
    public bool? NoRenames { get; set; }

    [BooleanCommandSwitch("no-rename-empty")]
    public bool? NoRenameEmpty { get; set; }

    [BooleanCommandSwitch("rename-empty")]
    public bool? RenameEmpty { get; set; }

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

    [CommandLongSwitch("output-directory")]
    public string? OutputDirectory { get; set; }

    [BooleanCommandSwitch("numbered")]
    public bool? Numbered { get; set; }

    [BooleanCommandSwitch("no-numbered")]
    public bool? NoNumbered { get; set; }

    [CommandLongSwitch("start-number")]
    public string? StartNumber { get; set; }

    [BooleanCommandSwitch("numbered-files")]
    public bool? NumberedFiles { get; set; }

    [BooleanCommandSwitch("keep-subject")]
    public bool? KeepSubject { get; set; }

    [BooleanCommandSwitch("signoff")]
    public bool? Signoff { get; set; }

    [BooleanCommandSwitch("stdout")]
    public bool? Stdout { get; set; }

    [CommandLongSwitch("attach")]
    public string? Attach { get; set; }

    [BooleanCommandSwitch("no-attach")]
    public bool? NoAttach { get; set; }

    [CommandLongSwitch("inline")]
    public string? Inline { get; set; }

    [CommandLongSwitch("thread")]
    public string? Thread { get; set; }

    [BooleanCommandSwitch("no-thread")]
    public bool? NoThread { get; set; }

    [CommandLongSwitch("in-reply-to")]
    public string? InReplyTo { get; set; }

    [BooleanCommandSwitch("ignore-if-in-upstream")]
    public bool? IgnoreIfInUpstream { get; set; }

    [BooleanCommandSwitch("always")]
    public bool? Always { get; set; }

    [CommandLongSwitch("cover-from-description")]
    public string? CoverFromDescription { get; set; }

    [CommandLongSwitch("subject-prefix")]
    public string? SubjectPrefix { get; set; }

    [CommandLongSwitch("filename-max-length")]
    public string? FilenameMaxLength { get; set; }

    [BooleanCommandSwitch("rfc")]
    public bool? Rfc { get; set; }

    [CommandLongSwitch("reroll-count")]
    public string? RerollCount { get; set; }

    [CommandLongSwitch("to")]
    public string? To { get; set; }

    [CommandLongSwitch("cc")]
    public string? Cc { get; set; }

    [BooleanCommandSwitch("from")]
    public bool? From { get; set; }

    [BooleanCommandSwitch("no-force-in-body-from")]
    public bool? NoForceInBodyFrom { get; set; }

    [BooleanCommandSwitch("force-in-body-from")]
    public bool? ForceInBodyFrom { get; set; }

    [CommandLongSwitch("add-header")]
    public string? AddHeader { get; set; }

    [BooleanCommandSwitch("no-cover-letter")]
    public bool? NoCoverLetter { get; set; }

    [BooleanCommandSwitch("cover-letter")]
    public bool? CoverLetter { get; set; }

    [BooleanCommandSwitch("encode-email-headers")]
    public bool? EncodeEmailHeaders { get; set; }

    [BooleanCommandSwitch("no-encode-email-headers")]
    public bool? NoEncodeEmailHeaders { get; set; }

    [CommandLongSwitch("interdiff")]
    public string? Interdiff { get; set; }

    [CommandLongSwitch("range-diff")]
    public string? RangeDiff { get; set; }

    [CommandLongSwitch("creation-factor")]
    public string? CreationFactor { get; set; }

    [CommandLongSwitch("notes")]
    public string? Notes { get; set; }

    [BooleanCommandSwitch("no-notes")]
    public bool? NoNotes { get; set; }

    [CommandLongSwitch("no-signature")]
    public string? NoSignature { get; set; }

    [CommandLongSwitch("signature")]
    public string? Signature { get; set; }

    [CommandLongSwitch("signature-file")]
    public string? SignatureFile { get; set; }

    [CommandLongSwitch("suffix")]
    public string? Suffix { get; set; }

    [BooleanCommandSwitch("quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("no-binary")]
    public bool? NoBinary { get; set; }

    [BooleanCommandSwitch("zero-commit")]
    public bool? ZeroCommit { get; set; }

    [CommandLongSwitch("no-base")]
    public string? NoBase { get; set; }

    [CommandLongSwitch("base")]
    public string? Base { get; set; }

    [BooleanCommandSwitch("root")]
    public bool? Root { get; set; }

    [BooleanCommandSwitch("progress")]
    public bool? Progress { get; set; }

}