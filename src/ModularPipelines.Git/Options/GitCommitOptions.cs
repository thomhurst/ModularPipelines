using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("commit")]
[ExcludeFromCodeCoverage]
public record GitCommitOptions : GitOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--patch")]
    public virtual bool? Patch { get; set; }

    [CliOption("--reuse-message", Format = OptionFormat.EqualsSeparated)]
    public string? ReuseMessage { get; set; }

    [CliOption("--reedit-message", Format = OptionFormat.EqualsSeparated)]
    public string? ReeditMessage { get; set; }

    [CliOption("--fixup", Format = OptionFormat.EqualsSeparated)]
    public string? Fixup { get; set; }

    [CliOption("--squash", Format = OptionFormat.EqualsSeparated)]
    public string? Squash { get; set; }

    [CliFlag("--reset-author")]
    public virtual bool? ResetAuthor { get; set; }

    [CliFlag("--short")]
    public virtual bool? Short { get; set; }

    [CliFlag("--branch")]
    public virtual bool? Branch { get; set; }

    [CliFlag("--porcelain")]
    public virtual bool? Porcelain { get; set; }

    [CliFlag("--long")]
    public virtual bool? Long { get; set; }

    [CliFlag("--null")]
    public virtual bool? Null { get; set; }

    [CliOption("--file", Format = OptionFormat.EqualsSeparated)]
    public string? File { get; set; }

    [CliOption("--author", Format = OptionFormat.EqualsSeparated)]
    public string? Author { get; set; }

    [CliOption("--date", Format = OptionFormat.EqualsSeparated)]
    public string? Date { get; set; }

    [CliOption("--message", Format = OptionFormat.EqualsSeparated)]
    public string? Message { get; set; }

    [CliOption("--template", Format = OptionFormat.EqualsSeparated)]
    public string? Template { get; set; }

    [CliFlag("--signoff")]
    public virtual bool? Signoff { get; set; }

    [CliFlag("--no-signoff")]
    public virtual bool? NoSignoff { get; set; }

    [CliOption("--trailer", Format = OptionFormat.EqualsSeparated)]
    public string? Trailer { get; set; }

    [CliFlag("--no-verify")]
    public virtual bool? NoVerify { get; set; }

    [CliFlag("--verify")]
    public virtual bool? Verify { get; set; }

    [CliFlag("--allow-empty")]
    public virtual bool? AllowEmpty { get; set; }

    [CliFlag("--allow-empty-message")]
    public virtual bool? AllowEmptyMessage { get; set; }

    [CliOption("--cleanup", Format = OptionFormat.EqualsSeparated)]
    public string? Cleanup { get; set; }

    [CliFlag("--edit")]
    public virtual bool? Edit { get; set; }

    [CliFlag("--no-edit")]
    public virtual bool? NoEdit { get; set; }

    [CliFlag("--amend")]
    public virtual bool? Amend { get; set; }

    [CliFlag("--no-post-rewrite")]
    public virtual bool? NoPostRewrite { get; set; }

    [CliFlag("--include")]
    public virtual bool? Include { get; set; }

    [CliFlag("--only")]
    public virtual bool? Only { get; set; }

    [CliOption("--pathspec-from-file", Format = OptionFormat.EqualsSeparated)]
    public string? PathspecFromFile { get; set; }

    [CliFlag("--pathspec-file-nul")]
    public virtual bool? PathspecFileNul { get; set; }

    [CliOption("--untracked-files", Format = OptionFormat.EqualsSeparated)]
    public string? UntrackedFiles { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("--status")]
    public virtual bool? Status { get; set; }

    [CliFlag("--no-status")]
    public virtual bool? NoStatus { get; set; }

    [CliOption("--gpg-sign", Format = OptionFormat.EqualsSeparated)]
    public string? GpgSign { get; set; }

    [CliFlag("--no-gpg-sign")]
    public virtual bool? NoGpgSign { get; set; }
}