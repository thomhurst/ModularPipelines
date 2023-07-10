using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("svn")]
public record GitSvnOptions : GitOptions
{
    [CommandLongSwitch("trunk")]
    public string? Trunk { get; set; }

    [CommandLongSwitch("tags")]
    public string? Tags { get; set; }

    [CommandLongSwitch("branches")]
    public string? Branches { get; set; }

    [BooleanCommandSwitch("stdlayout")]
    public bool? Stdlayout { get; set; }

    [BooleanCommandSwitch("no-metadata")]
    public bool? NoMetadata { get; set; }

    [BooleanCommandSwitch("use-svm-props")]
    public bool? UseSvmProps { get; set; }

    [BooleanCommandSwitch("use-svnsync-props")]
    public bool? UseSvnsyncProps { get; set; }

    [CommandLongSwitch("rewrite-root")]
    public string? RewriteRoot { get; set; }

    [CommandLongSwitch("rewrite-uuid")]
    public string? RewriteUuid { get; set; }

    [CommandLongSwitch("username")]
    public string? Username { get; set; }

    [CommandLongSwitch("prefix")]
    public string? Prefix { get; set; }

    [CommandLongSwitch("ignore-refs")]
    public string? IgnoreRefs { get; set; }

    [CommandLongSwitch("ignore-paths")]
    public string? IgnorePaths { get; set; }

    [CommandLongSwitch("include-paths")]
    public string? IncludePaths { get; set; }

    [BooleanCommandSwitch("no-minimize-url")]
    public bool? NoMinimizeUrl { get; set; }

    [BooleanCommandSwitch("localtime")]
    public bool? Localtime { get; set; }

    [BooleanCommandSwitch("parent")]
    public bool? Parent { get; set; }

    [CommandLongSwitch("log-window-size")]
    public string? LogWindowSize { get; set; }

    [BooleanCommandSwitch("preserve-empty-dirs")]
    public bool? PreserveEmptyDirs { get; set; }

    [CommandLongSwitch("placeholder-filename")]
    public string? PlaceholderFilename { get; set; }

    [BooleanCommandSwitch("local")]
    public bool? Local { get; set; }

    [BooleanCommandSwitch("no-rebase")]
    public bool? NoRebase { get; set; }

    [CommandLongSwitch("commit-url")]
    public string? CommitUrl { get; set; }

    [CommandLongSwitch("mergeinfo")]
    public string? Mergeinfo { get; set; }

    [BooleanCommandSwitch("interactive")]
    public bool? Interactive { get; set; }

    [BooleanCommandSwitch("message")]
    public bool? Message { get; set; }

    [BooleanCommandSwitch("tag")]
    public bool? Tag { get; set; }

    [CommandLongSwitch("destination")]
    public string? Destination { get; set; }

    [BooleanCommandSwitch("parents")]
    public bool? Parents { get; set; }

    [CommandLongSwitch("revision")]
    public string? Revision { get; set; }

    [BooleanCommandSwitch("verbose")]
    public bool? Verbose { get; set; }

    [CommandLongSwitch("limit")]
    public string? Limit { get; set; }

    [BooleanCommandSwitch("incremental")]
    public bool? Incremental { get; set; }

    [BooleanCommandSwitch("show-commit")]
    public bool? ShowCommit { get; set; }

    [BooleanCommandSwitch("oneline")]
    public bool? Oneline { get; set; }

    [BooleanCommandSwitch("git-format")]
    public bool? GitFormat { get; set; }

    [BooleanCommandSwitch("before")]
    public bool? Before { get; set; }

    [BooleanCommandSwitch("after")]
    public bool? After { get; set; }

    [CommandLongSwitch("file")]
    public string? File { get; set; }

    [BooleanCommandSwitch("shared")]
    public bool? Shared { get; set; }

    [CommandLongSwitch("template")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("stdin")]
    public bool? Stdin { get; set; }

    [BooleanCommandSwitch("rmdir")]
    public bool? Rmdir { get; set; }

    [BooleanCommandSwitch("edit")]
    public bool? Edit { get; set; }

    [BooleanCommandSwitch("find-copies-harder")]
    public bool? FindCopiesHarder { get; set; }

    [CommandLongSwitch("authors-file")]
    public string? AuthorsFile { get; set; }

    [CommandLongSwitch("authors-prog")]
    public string? AuthorsProg { get; set; }

    [BooleanCommandSwitch("quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("merge")]
    public bool? Merge { get; set; }

    [CommandLongSwitch("strategy")]
    public string? Strategy { get; set; }

    [BooleanCommandSwitch("rebase-merges")]
    public bool? RebaseMerges { get; set; }

    [BooleanCommandSwitch("dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("use-log-author")]
    public bool? UseLogAuthor { get; set; }

    [BooleanCommandSwitch("add-author-from")]
    public bool? AddAuthorFrom { get; set; }

    [CommandLongSwitch("id")]
    public string? Id { get; set; }

    [CommandLongSwitch("svn-remote")]
    public string? SvnRemote { get; set; }

    [BooleanCommandSwitch("follow-parent")]
    public bool? FollowParent { get; set; }

}