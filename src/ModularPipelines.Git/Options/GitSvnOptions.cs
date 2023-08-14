using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("svn")]
public record GitSvnOptions : GitOptions
{
    [CommandEqualsSeparatorSwitch("--trunk")]
    public string? Trunk { get; set; }

    [CommandEqualsSeparatorSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandEqualsSeparatorSwitch("--branches")]
    public string? Branches { get; set; }

    [BooleanCommandSwitch("--stdlayout")]
    public bool? Stdlayout { get; set; }

    [BooleanCommandSwitch("--no-metadata")]
    public bool? NoMetadata { get; set; }

    [BooleanCommandSwitch("--use-svm-props")]
    public bool? UseSvmProps { get; set; }

    [BooleanCommandSwitch("--use-svnsync-props")]
    public bool? UseSvnsyncProps { get; set; }

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
    public bool? NoMinimizeUrl { get; set; }

    [BooleanCommandSwitch("--localtime")]
    public bool? Localtime { get; set; }

    [BooleanCommandSwitch("--parent")]
    public bool? Parent { get; set; }

    [CommandEqualsSeparatorSwitch("--log-window-size")]
    public string? LogWindowSize { get; set; }

    [BooleanCommandSwitch("--preserve-empty-dirs")]
    public bool? PreserveEmptyDirs { get; set; }

    [CommandEqualsSeparatorSwitch("--placeholder-filename")]
    public string? PlaceholderFilename { get; set; }

    [BooleanCommandSwitch("--local")]
    public bool? Local { get; set; }

    [BooleanCommandSwitch("--no-rebase")]
    public bool? NoRebase { get; set; }

    [CommandEqualsSeparatorSwitch("--commit-url")]
    public string? CommitUrl { get; set; }

    [CommandEqualsSeparatorSwitch("--mergeinfo")]
    public string? Mergeinfo { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [BooleanCommandSwitch("--message")]
    public bool? Message { get; set; }

    [BooleanCommandSwitch("--tag")]
    public bool? Tag { get; set; }

    [CommandEqualsSeparatorSwitch("--destination")]
    public string? Destination { get; set; }

    [BooleanCommandSwitch("--parents")]
    public bool? Parents { get; set; }

    [CommandEqualsSeparatorSwitch("--revision")]
    public string? Revision { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [CommandEqualsSeparatorSwitch("--limit")]
    public string? Limit { get; set; }

    [BooleanCommandSwitch("--incremental")]
    public bool? Incremental { get; set; }

    [BooleanCommandSwitch("--show-commit")]
    public bool? ShowCommit { get; set; }

    [BooleanCommandSwitch("--oneline")]
    public bool? Oneline { get; set; }

    [BooleanCommandSwitch("--git-format")]
    public bool? GitFormat { get; set; }

    [BooleanCommandSwitch("--before")]
    public bool? Before { get; set; }

    [BooleanCommandSwitch("--after")]
    public bool? After { get; set; }

    [CommandEqualsSeparatorSwitch("--file")]
    public string? File { get; set; }

    [BooleanCommandSwitch("--shared")]
    public bool? Shared { get; set; }

    [CommandEqualsSeparatorSwitch("--template")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("--stdin")]
    public bool? Stdin { get; set; }

    [BooleanCommandSwitch("--rmdir")]
    public bool? Rmdir { get; set; }

    [BooleanCommandSwitch("--edit")]
    public bool? Edit { get; set; }

    [BooleanCommandSwitch("--find-copies-harder")]
    public bool? FindCopiesHarder { get; set; }

    [CommandEqualsSeparatorSwitch("--authors-file")]
    public string? AuthorsFile { get; set; }

    [CommandEqualsSeparatorSwitch("--authors-prog")]
    public string? AuthorsProg { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--merge")]
    public bool? Merge { get; set; }

    [CommandEqualsSeparatorSwitch("--strategy")]
    public string? Strategy { get; set; }

    [BooleanCommandSwitch("--rebase-merges")]
    public bool? RebaseMerges { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--use-log-author")]
    public bool? UseLogAuthor { get; set; }

    [BooleanCommandSwitch("--add-author-from")]
    public bool? AddAuthorFrom { get; set; }

    [CommandEqualsSeparatorSwitch("--id")]
    public string? Id { get; set; }

    [CommandEqualsSeparatorSwitch("--svn-remote")]
    public string? SvnRemote { get; set; }

    [BooleanCommandSwitch("--follow-parent")]
    public bool? FollowParent { get; set; }
}