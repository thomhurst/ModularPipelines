using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("format-patch")]
[ExcludeFromCodeCoverage]
public record GitFormatPatchOptions : GitOptions
{
    [CliFlag("--no-stat")]
    public virtual bool? NoStat { get; set; }

    [CliOption("--unified", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Unified { get; set; }

    [CliOption("--output", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Output { get; set; }

    [CliOption("--output-indicator-new", Format = OptionFormat.EqualsSeparated)]
    public virtual string? OutputIndicatorNew { get; set; }

    [CliOption("--output-indicator-old", Format = OptionFormat.EqualsSeparated)]
    public virtual string? OutputIndicatorOld { get; set; }

    [CliOption("--output-indicator-context", Format = OptionFormat.EqualsSeparated)]
    public virtual string? OutputIndicatorContext { get; set; }

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
    public virtual string? Anchored { get; set; }

    [CliFlag("--diff-algorithm")]
    public virtual bool? DiffAlgorithm { get; set; }

    [CliOption("--stat", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Stat { get; set; }

    [CliFlag("--compact-summary")]
    public virtual bool? CompactSummary { get; set; }

    [CliFlag("--numstat")]
    public virtual bool? Numstat { get; set; }

    [CliFlag("--shortstat")]
    public virtual bool? Shortstat { get; set; }

    [CliOption("--dirstat", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Dirstat { get; set; }

    [CliFlag("--cumulative")]
    public virtual bool? Cumulative { get; set; }

    [CliOption("--dirstat-by-file", Format = OptionFormat.EqualsSeparated)]
    public virtual string? DirstatByFile { get; set; }

    [CliFlag("--summary")]
    public virtual bool? Summary { get; set; }

    [CliFlag("--no-renames")]
    public virtual bool? NoRenames { get; set; }

    [CliFlag("--no-rename-empty")]
    public virtual bool? NoRenameEmpty { get; set; }

    [CliFlag("--rename-empty")]
    public virtual bool? RenameEmpty { get; set; }

    [CliFlag("--full-index")]
    public virtual bool? FullIndex { get; set; }

    [CliFlag("--binary")]
    public virtual bool? Binary { get; set; }

    [CliOption("--abbrev", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Abbrev { get; set; }

    [CliOption("--break-rewrites", Format = OptionFormat.EqualsSeparated)]
    public virtual string? BreakRewrites { get; set; }

    [CliOption("--find-renames", Format = OptionFormat.EqualsSeparated)]
    public virtual string? FindRenames { get; set; }

    [CliOption("--find-copies", Format = OptionFormat.EqualsSeparated)]
    public virtual string? FindCopies { get; set; }

    [CliFlag("--find-copies-harder")]
    public virtual bool? FindCopiesHarder { get; set; }

    [CliFlag("--irreversible-delete")]
    public virtual bool? IrreversibleDelete { get; set; }

    [CliOption("--skip-to", Format = OptionFormat.EqualsSeparated)]
    public virtual string? SkipTo { get; set; }

    [CliOption("--rotate-to", Format = OptionFormat.EqualsSeparated)]
    public virtual string? RotateTo { get; set; }

    [CliOption("--relative", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Relative { get; set; }

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
    public virtual string? IgnoreMatchingLines { get; set; }

    [CliOption("--inter-hunk-context", Format = OptionFormat.EqualsSeparated)]
    public virtual string? InterHunkContext { get; set; }

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
    public virtual string? IgnoreSubmodules { get; set; }

    [CliOption("--src-prefix", Format = OptionFormat.EqualsSeparated)]
    public virtual string? SrcPrefix { get; set; }

    [CliOption("--dst-prefix", Format = OptionFormat.EqualsSeparated)]
    public virtual string? DstPrefix { get; set; }

    [CliFlag("--no-prefix")]
    public virtual bool? NoPrefix { get; set; }

    [CliFlag("--default-prefix")]
    public virtual bool? DefaultPrefix { get; set; }

    [CliOption("--line-prefix", Format = OptionFormat.EqualsSeparated)]
    public virtual string? LinePrefix { get; set; }

    [CliFlag("--ita-invisible-in-index")]
    public virtual bool? ItaInvisibleInIndex { get; set; }

    [CliOption("--output-directory", Format = OptionFormat.EqualsSeparated)]
    public virtual string? OutputDirectory { get; set; }

    [CliFlag("--numbered")]
    public virtual bool? Numbered { get; set; }

    [CliFlag("--no-numbered")]
    public virtual bool? NoNumbered { get; set; }

    [CliOption("--start-number", Format = OptionFormat.EqualsSeparated)]
    public virtual string? StartNumber { get; set; }

    [CliFlag("--numbered-files")]
    public virtual bool? NumberedFiles { get; set; }

    [CliFlag("--keep-subject")]
    public virtual bool? KeepSubject { get; set; }

    [CliFlag("--signoff")]
    public virtual bool? Signoff { get; set; }

    [CliFlag("--stdout")]
    public virtual bool? Stdout { get; set; }

    [CliOption("--attach", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Attach { get; set; }

    [CliFlag("--no-attach")]
    public virtual bool? NoAttach { get; set; }

    [CliOption("--inline", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Inline { get; set; }

    [CliOption("--thread", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Thread { get; set; }

    [CliFlag("--no-thread")]
    public virtual bool? NoThread { get; set; }

    [CliOption("--in-reply-to", Format = OptionFormat.EqualsSeparated)]
    public virtual string? InReplyTo { get; set; }

    [CliFlag("--ignore-if-in-upstream")]
    public virtual bool? IgnoreIfInUpstream { get; set; }

    [CliFlag("--always")]
    public virtual bool? Always { get; set; }

    [CliOption("--cover-from-description", Format = OptionFormat.EqualsSeparated)]
    public virtual string? CoverFromDescription { get; set; }

    [CliOption("--subject-prefix", Format = OptionFormat.EqualsSeparated)]
    public virtual string? SubjectPrefix { get; set; }

    [CliOption("--filename-max-length", Format = OptionFormat.EqualsSeparated)]
    public virtual string? FilenameMaxLength { get; set; }

    [CliFlag("--rfc")]
    public virtual bool? Rfc { get; set; }

    [CliOption("--reroll-count", Format = OptionFormat.EqualsSeparated)]
    public virtual string? RerollCount { get; set; }

    [CliOption("--to", Format = OptionFormat.EqualsSeparated)]
    public virtual string? To { get; set; }

    [CliOption("--cc", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Cc { get; set; }

    [CliFlag("--from")]
    public virtual bool? From { get; set; }

    [CliFlag("--no-force-in-body-from")]
    public virtual bool? NoForceInBodyFrom { get; set; }

    [CliFlag("--force-in-body-from")]
    public virtual bool? ForceInBodyFrom { get; set; }

    [CliOption("--add-header", Format = OptionFormat.EqualsSeparated)]
    public virtual string? AddHeader { get; set; }

    [CliFlag("--no-cover-letter")]
    public virtual bool? NoCoverLetter { get; set; }

    [CliFlag("--cover-letter")]
    public virtual bool? CoverLetter { get; set; }

    [CliFlag("--encode-email-headers")]
    public virtual bool? EncodeEmailHeaders { get; set; }

    [CliFlag("--no-encode-email-headers")]
    public virtual bool? NoEncodeEmailHeaders { get; set; }

    [CliOption("--interdiff", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Interdiff { get; set; }

    [CliOption("--range-diff", Format = OptionFormat.EqualsSeparated)]
    public virtual string? RangeDiff { get; set; }

    [CliOption("--creation-factor", Format = OptionFormat.EqualsSeparated)]
    public virtual string? CreationFactor { get; set; }

    [CliOption("--notes", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Notes { get; set; }

    [CliFlag("--no-notes")]
    public virtual bool? NoNotes { get; set; }

    [CliOption("--no-signature", Format = OptionFormat.EqualsSeparated)]
    public virtual string? NoSignature { get; set; }

    [CliOption("--signature", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Signature { get; set; }

    [CliOption("--signature-file", Format = OptionFormat.EqualsSeparated)]
    public virtual string? SignatureFile { get; set; }

    [CliOption("--suffix", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Suffix { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--no-binary")]
    public virtual bool? NoBinary { get; set; }

    [CliFlag("--zero-commit")]
    public virtual bool? ZeroCommit { get; set; }

    [CliOption("--no-base", Format = OptionFormat.EqualsSeparated)]
    public virtual string? NoBase { get; set; }

    [CliOption("--base", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Base { get; set; }

    [CliFlag("--root")]
    public virtual bool? Root { get; set; }

    [CliFlag("--progress")]
    public virtual bool? Progress { get; set; }
}