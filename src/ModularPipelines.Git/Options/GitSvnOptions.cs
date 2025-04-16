using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("svn")]
[ExcludeFromCodeCoverage]
public record GitSvnOptions : GitOptions
{
    [CommandEqualsSeparatorSwitch("--trunk")]
    public string? Trunk { get; set; }

    [CommandEqualsSeparatorSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandEqualsSeparatorSwitch("--branches")]
    public string? Branches { get; set; }

    [BooleanCommandSwitch("--stdlayout")]
    public virtual bool? Stdlayout { get; set; }

    [BooleanCommandSwitch("--no-metadata")]
    public virtual bool? NoMetadata { get; set; }

    [BooleanCommandSwitch("--use-svm-props")]
    public virtual bool? UseSvmProps { get; set; }

    [BooleanCommandSwitch("--use-svnsync-props")]
    public virtual bool? UseSvnsyncProps { get; set; }

    [CommandEqualsSeparatorSwitch("--rewrite-root")]
    public string? RewriteRoot { get; set; }

    [CommandEqualsSeparatorSwitch("--rewrite-uuid")]
    public string? RewriteUuid { get; set; }

    [CommandEqualsSeparatorSwitch("--username")]
    public string? Username { get; set; }

    [CommandEqualsSeparatorSwitch("--prefix")]
    public string? Prefix { get; set; }

    [CommandEqualsSeparatorSwitch("--ignore-refs")]
    public string? IgnoreRefs { get; set; }

    [CommandEqualsSeparatorSwitch("--ignore-paths")]
    public string? IgnorePaths { get; set; }

    [CommandEqualsSeparatorSwitch("--include-paths")]
    public string? IncludePaths { get; set; }

    [BooleanCommandSwitch("--no-minimize-url")]
    public virtual bool? NoMinimizeUrl { get; set; }

    [BooleanCommandSwitch("--localtime")]
    public virtual bool? Localtime { get; set; }

    [BooleanCommandSwitch("--parent")]
    public virtual bool? Parent { get; set; }

    [CommandEqualsSeparatorSwitch("--log-window-size")]
    public string? LogWindowSize { get; set; }

    [BooleanCommandSwitch("--preserve-empty-dirs")]
    public virtual bool? PreserveEmptyDirs { get; set; }

    [CommandEqualsSeparatorSwitch("--placeholder-filename")]
    public string? PlaceholderFilename { get; set; }

    [BooleanCommandSwitch("--local")]
    public virtual bool? Local { get; set; }

    [BooleanCommandSwitch("--no-rebase")]
    public virtual bool? NoRebase { get; set; }

    [CommandEqualsSeparatorSwitch("--commit-url")]
    public string? CommitUrl { get; set; }

    [CommandEqualsSeparatorSwitch("--mergeinfo")]
    public string? Mergeinfo { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [BooleanCommandSwitch("--message")]
    public virtual bool? Message { get; set; }

    [BooleanCommandSwitch("--tag")]
    public virtual bool? Tag { get; set; }

    [CommandEqualsSeparatorSwitch("--destination")]
    public string? Destination { get; set; }

    [BooleanCommandSwitch("--parents")]
    public virtual bool? Parents { get; set; }

    [CommandEqualsSeparatorSwitch("--revision")]
    public string? Revision { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CommandEqualsSeparatorSwitch("--limit")]
    public string? Limit { get; set; }

    [BooleanCommandSwitch("--incremental")]
    public virtual bool? Incremental { get; set; }

    [BooleanCommandSwitch("--show-commit")]
    public virtual bool? ShowCommit { get; set; }

    [BooleanCommandSwitch("--oneline")]
    public virtual bool? Oneline { get; set; }

    [BooleanCommandSwitch("--git-format")]
    public virtual bool? GitFormat { get; set; }

    [BooleanCommandSwitch("--before")]
    public virtual bool? Before { get; set; }

    [BooleanCommandSwitch("--after")]
    public virtual bool? After { get; set; }

    [CommandEqualsSeparatorSwitch("--file")]
    public string? File { get; set; }

    [BooleanCommandSwitch("--shared")]
    public virtual bool? Shared { get; set; }

    [CommandEqualsSeparatorSwitch("--template")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("--stdin")]
    public virtual bool? Stdin { get; set; }

    [BooleanCommandSwitch("--rmdir")]
    public virtual bool? Rmdir { get; set; }

    [BooleanCommandSwitch("--edit")]
    public virtual bool? Edit { get; set; }

    [BooleanCommandSwitch("--find-copies-harder")]
    public virtual bool? FindCopiesHarder { get; set; }

    [CommandEqualsSeparatorSwitch("--authors-file")]
    public string? AuthorsFile { get; set; }

    [CommandEqualsSeparatorSwitch("--authors-prog")]
    public string? AuthorsProg { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--merge")]
    public virtual bool? Merge { get; set; }

    [CommandEqualsSeparatorSwitch("--strategy")]
    public string? Strategy { get; set; }

    [BooleanCommandSwitch("--rebase-merges")]
    public virtual bool? RebaseMerges { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("--use-log-author")]
    public virtual bool? UseLogAuthor { get; set; }

    [BooleanCommandSwitch("--add-author-from")]
    public virtual bool? AddAuthorFrom { get; set; }

    [CommandEqualsSeparatorSwitch("--id")]
    public string? Id { get; set; }

    [CommandEqualsSeparatorSwitch("--svn-remote")]
    public string? SvnRemote { get; set; }

    [BooleanCommandSwitch("--follow-parent")]
    public virtual bool? FollowParent { get; set; }
}