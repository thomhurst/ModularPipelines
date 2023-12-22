using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("commit")]
[ExcludeFromCodeCoverage]
public record GitCommitOptions : GitOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--patch")]
    public bool? Patch { get; set; }

    [CommandEqualsSeparatorSwitch("--reuse-message")]
    public string? ReuseMessage { get; set; }

    [CommandEqualsSeparatorSwitch("--reedit-message")]
    public string? ReeditMessage { get; set; }

    [CommandEqualsSeparatorSwitch("--fixup")]
    public string? Fixup { get; set; }

    [CommandEqualsSeparatorSwitch("--squash")]
    public string? Squash { get; set; }

    [BooleanCommandSwitch("--reset-author")]
    public bool? ResetAuthor { get; set; }

    [BooleanCommandSwitch("--short")]
    public bool? Short { get; set; }

    [BooleanCommandSwitch("--branch")]
    public bool? Branch { get; set; }

    [BooleanCommandSwitch("--porcelain")]
    public bool? Porcelain { get; set; }

    [BooleanCommandSwitch("--long")]
    public bool? Long { get; set; }

    [BooleanCommandSwitch("--null")]
    public bool? Null { get; set; }

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
    public bool? Signoff { get; set; }

    [BooleanCommandSwitch("--no-signoff")]
    public bool? NoSignoff { get; set; }

    [CommandEqualsSeparatorSwitch("--trailer")]
    public string? Trailer { get; set; }

    [BooleanCommandSwitch("--no-verify")]
    public bool? NoVerify { get; set; }

    [BooleanCommandSwitch("--verify")]
    public bool? Verify { get; set; }

    [BooleanCommandSwitch("--allow-empty")]
    public bool? AllowEmpty { get; set; }

    [BooleanCommandSwitch("--allow-empty-message")]
    public bool? AllowEmptyMessage { get; set; }

    [CommandEqualsSeparatorSwitch("--cleanup")]
    public string? Cleanup { get; set; }

    [BooleanCommandSwitch("--edit")]
    public bool? Edit { get; set; }

    [BooleanCommandSwitch("--no-edit")]
    public bool? NoEdit { get; set; }

    [BooleanCommandSwitch("--amend")]
    public bool? Amend { get; set; }

    [BooleanCommandSwitch("--no-post-rewrite")]
    public bool? NoPostRewrite { get; set; }

    [BooleanCommandSwitch("--include")]
    public bool? Include { get; set; }

    [BooleanCommandSwitch("--only")]
    public bool? Only { get; set; }

    [CommandEqualsSeparatorSwitch("--pathspec-from-file")]
    public string? PathspecFromFile { get; set; }

    [BooleanCommandSwitch("--pathspec-file-nul")]
    public bool? PathspecFileNul { get; set; }

    [CommandEqualsSeparatorSwitch("--untracked-files")]
    public string? UntrackedFiles { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--status")]
    public bool? Status { get; set; }

    [BooleanCommandSwitch("--no-status")]
    public bool? NoStatus { get; set; }

    [CommandEqualsSeparatorSwitch("--gpg-sign")]
    public string? GpgSign { get; set; }

    [BooleanCommandSwitch("--no-gpg-sign")]
    public bool? NoGpgSign { get; set; }
}