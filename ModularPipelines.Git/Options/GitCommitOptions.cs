using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("commit")]
public record GitCommitOptions : GitOptions
{
    [BooleanCommandSwitch("all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("patch")]
    public bool? Patch { get; set; }

    [CommandLongSwitch("reuse-message")]
    public string? ReuseMessage { get; set; }

    [CommandLongSwitch("reedit-message")]
    public string? ReeditMessage { get; set; }

    [CommandLongSwitch("fixup")]
    public string? Fixup { get; set; }

    [CommandLongSwitch("squash")]
    public string? Squash { get; set; }

    [BooleanCommandSwitch("reset-author")]
    public bool? ResetAuthor { get; set; }

    [BooleanCommandSwitch("short")]
    public bool? Short { get; set; }

    [BooleanCommandSwitch("branch")]
    public bool? Branch { get; set; }

    [BooleanCommandSwitch("porcelain")]
    public bool? Porcelain { get; set; }

    [BooleanCommandSwitch("long")]
    public bool? Long { get; set; }

    [BooleanCommandSwitch("null")]
    public bool? Null { get; set; }

    [CommandLongSwitch("file")]
    public string? File { get; set; }

    [CommandLongSwitch("author")]
    public string? Author { get; set; }

    [CommandLongSwitch("date")]
    public string? Date { get; set; }

    [CommandLongSwitch("message")]
    public string? Message { get; set; }

    [CommandLongSwitch("template")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("signoff")]
    public bool? Signoff { get; set; }

    [BooleanCommandSwitch("no-signoff")]
    public bool? NoSignoff { get; set; }

    [CommandLongSwitch("trailer")]
    public string? Trailer { get; set; }

    [BooleanCommandSwitch("no-verify")]
    public bool? NoVerify { get; set; }

    [BooleanCommandSwitch("verify")]
    public bool? Verify { get; set; }

    [BooleanCommandSwitch("allow-empty")]
    public bool? AllowEmpty { get; set; }

    [BooleanCommandSwitch("allow-empty-message")]
    public bool? AllowEmptyMessage { get; set; }

    [CommandLongSwitch("cleanup")]
    public string? Cleanup { get; set; }

    [BooleanCommandSwitch("edit")]
    public bool? Edit { get; set; }

    [BooleanCommandSwitch("no-edit")]
    public bool? NoEdit { get; set; }

    [BooleanCommandSwitch("amend")]
    public bool? Amend { get; set; }

    [BooleanCommandSwitch("no-post-rewrite")]
    public bool? NoPostRewrite { get; set; }

    [BooleanCommandSwitch("include")]
    public bool? Include { get; set; }

    [BooleanCommandSwitch("only")]
    public bool? Only { get; set; }

    [CommandLongSwitch("pathspec-from-file")]
    public string? PathspecFromFile { get; set; }

    [BooleanCommandSwitch("pathspec-file-nul")]
    public bool? PathspecFileNul { get; set; }

    [CommandLongSwitch("untracked-files")]
    public string? UntrackedFiles { get; set; }

    [BooleanCommandSwitch("verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("status")]
    public bool? Status { get; set; }

    [BooleanCommandSwitch("no-status")]
    public bool? NoStatus { get; set; }

    [CommandLongSwitch("gpg-sign")]
    public string? GpgSign { get; set; }

    [BooleanCommandSwitch("no-gpg-sign")]
    public bool? NoGpgSign { get; set; }

}