using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("commit")]
[ExcludeFromCodeCoverage]
public record GitCommitOptions : GitOptions
{
    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--patch")]
    public virtual bool? Patch { get; set; }

    [CommandEqualsSeparatorSwitch("--reuse-message")]
    public string? ReuseMessage { get; set; }

    [CommandEqualsSeparatorSwitch("--reedit-message")]
    public string? ReeditMessage { get; set; }

    [CommandEqualsSeparatorSwitch("--fixup")]
    public string? Fixup { get; set; }

    [CommandEqualsSeparatorSwitch("--squash")]
    public string? Squash { get; set; }

    [BooleanCommandSwitch("--reset-author")]
    public virtual bool? ResetAuthor { get; set; }

    [BooleanCommandSwitch("--short")]
    public virtual bool? Short { get; set; }

    [BooleanCommandSwitch("--branch")]
    public virtual bool? Branch { get; set; }

    [BooleanCommandSwitch("--porcelain")]
    public virtual bool? Porcelain { get; set; }

    [BooleanCommandSwitch("--long")]
    public virtual bool? Long { get; set; }

    [BooleanCommandSwitch("--null")]
    public virtual bool? Null { get; set; }

    [CommandEqualsSeparatorSwitch("--file")]
    public string? File { get; set; }

    [CommandEqualsSeparatorSwitch("--author")]
    public string? Author { get; set; }

    [CommandEqualsSeparatorSwitch("--date")]
    public string? Date { get; set; }

    [CommandEqualsSeparatorSwitch("--message")]
    public string? Message { get; set; }

    [CommandEqualsSeparatorSwitch("--template")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("--signoff")]
    public virtual bool? Signoff { get; set; }

    [BooleanCommandSwitch("--no-signoff")]
    public virtual bool? NoSignoff { get; set; }

    [CommandEqualsSeparatorSwitch("--trailer")]
    public string? Trailer { get; set; }

    [BooleanCommandSwitch("--no-verify")]
    public virtual bool? NoVerify { get; set; }

    [BooleanCommandSwitch("--verify")]
    public virtual bool? Verify { get; set; }

    [BooleanCommandSwitch("--allow-empty")]
    public virtual bool? AllowEmpty { get; set; }

    [BooleanCommandSwitch("--allow-empty-message")]
    public virtual bool? AllowEmptyMessage { get; set; }

    [CommandEqualsSeparatorSwitch("--cleanup")]
    public string? Cleanup { get; set; }

    [BooleanCommandSwitch("--edit")]
    public virtual bool? Edit { get; set; }

    [BooleanCommandSwitch("--no-edit")]
    public virtual bool? NoEdit { get; set; }

    [BooleanCommandSwitch("--amend")]
    public virtual bool? Amend { get; set; }

    [BooleanCommandSwitch("--no-post-rewrite")]
    public virtual bool? NoPostRewrite { get; set; }

    [BooleanCommandSwitch("--include")]
    public virtual bool? Include { get; set; }

    [BooleanCommandSwitch("--only")]
    public virtual bool? Only { get; set; }

    [CommandEqualsSeparatorSwitch("--pathspec-from-file")]
    public string? PathspecFromFile { get; set; }

    [BooleanCommandSwitch("--pathspec-file-nul")]
    public virtual bool? PathspecFileNul { get; set; }

    [CommandEqualsSeparatorSwitch("--untracked-files")]
    public string? UntrackedFiles { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("--status")]
    public virtual bool? Status { get; set; }

    [BooleanCommandSwitch("--no-status")]
    public virtual bool? NoStatus { get; set; }

    [CommandEqualsSeparatorSwitch("--gpg-sign")]
    public string? GpgSign { get; set; }

    [BooleanCommandSwitch("--no-gpg-sign")]
    public virtual bool? NoGpgSign { get; set; }
}