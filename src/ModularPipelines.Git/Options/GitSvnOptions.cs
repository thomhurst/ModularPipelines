using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("svn")]
[ExcludeFromCodeCoverage]
public record GitSvnOptions : GitOptions
{
    [CliOption("--trunk", Format = OptionFormat.EqualsSeparated)]
    public string? Trunk { get; set; }

    [CliOption("--tags", Format = OptionFormat.EqualsSeparated)]
    public string? Tags { get; set; }

    [CliOption("--branches", Format = OptionFormat.EqualsSeparated)]
    public string? Branches { get; set; }

    [CliFlag("--stdlayout")]
    public virtual bool? Stdlayout { get; set; }

    [CliFlag("--no-metadata")]
    public virtual bool? NoMetadata { get; set; }

    [CliFlag("--use-svm-props")]
    public virtual bool? UseSvmProps { get; set; }

    [CliFlag("--use-svnsync-props")]
    public virtual bool? UseSvnsyncProps { get; set; }

    [CliOption("--rewrite-root", Format = OptionFormat.EqualsSeparated)]
    public string? RewriteRoot { get; set; }

    [CliOption("--rewrite-uuid", Format = OptionFormat.EqualsSeparated)]
    public string? RewriteUuid { get; set; }

    [CliOption("--username", Format = OptionFormat.EqualsSeparated)]
    public string? Username { get; set; }

    [CliOption("--prefix", Format = OptionFormat.EqualsSeparated)]
    public string? Prefix { get; set; }

    [CliOption("--ignore-refs", Format = OptionFormat.EqualsSeparated)]
    public string? IgnoreRefs { get; set; }

    [CliOption("--ignore-paths", Format = OptionFormat.EqualsSeparated)]
    public string? IgnorePaths { get; set; }

    [CliOption("--include-paths", Format = OptionFormat.EqualsSeparated)]
    public string? IncludePaths { get; set; }

    [CliFlag("--no-minimize-url")]
    public virtual bool? NoMinimizeUrl { get; set; }

    [CliFlag("--localtime")]
    public virtual bool? Localtime { get; set; }

    [CliFlag("--parent")]
    public virtual bool? Parent { get; set; }

    [CliOption("--log-window-size", Format = OptionFormat.EqualsSeparated)]
    public string? LogWindowSize { get; set; }

    [CliFlag("--preserve-empty-dirs")]
    public virtual bool? PreserveEmptyDirs { get; set; }

    [CliOption("--placeholder-filename", Format = OptionFormat.EqualsSeparated)]
    public string? PlaceholderFilename { get; set; }

    [CliFlag("--local")]
    public virtual bool? Local { get; set; }

    [CliFlag("--no-rebase")]
    public virtual bool? NoRebase { get; set; }

    [CliOption("--commit-url", Format = OptionFormat.EqualsSeparated)]
    public string? CommitUrl { get; set; }

    [CliOption("--mergeinfo", Format = OptionFormat.EqualsSeparated)]
    public string? Mergeinfo { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliFlag("--message")]
    public virtual bool? Message { get; set; }

    [CliFlag("--tag")]
    public virtual bool? Tag { get; set; }

    [CliOption("--destination", Format = OptionFormat.EqualsSeparated)]
    public string? Destination { get; set; }

    [CliFlag("--parents")]
    public virtual bool? Parents { get; set; }

    [CliOption("--revision", Format = OptionFormat.EqualsSeparated)]
    public string? Revision { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliOption("--limit", Format = OptionFormat.EqualsSeparated)]
    public string? Limit { get; set; }

    [CliFlag("--incremental")]
    public virtual bool? Incremental { get; set; }

    [CliFlag("--show-commit")]
    public virtual bool? ShowCommit { get; set; }

    [CliFlag("--oneline")]
    public virtual bool? Oneline { get; set; }

    [CliFlag("--git-format")]
    public virtual bool? GitFormat { get; set; }

    [CliFlag("--before")]
    public virtual bool? Before { get; set; }

    [CliFlag("--after")]
    public virtual bool? After { get; set; }

    [CliOption("--file", Format = OptionFormat.EqualsSeparated)]
    public string? File { get; set; }

    [CliFlag("--shared")]
    public virtual bool? Shared { get; set; }

    [CliOption("--template", Format = OptionFormat.EqualsSeparated)]
    public string? Template { get; set; }

    [CliFlag("--stdin")]
    public virtual bool? Stdin { get; set; }

    [CliFlag("--rmdir")]
    public virtual bool? Rmdir { get; set; }

    [CliFlag("--edit")]
    public virtual bool? Edit { get; set; }

    [CliFlag("--find-copies-harder")]
    public virtual bool? FindCopiesHarder { get; set; }

    [CliOption("--authors-file", Format = OptionFormat.EqualsSeparated)]
    public string? AuthorsFile { get; set; }

    [CliOption("--authors-prog", Format = OptionFormat.EqualsSeparated)]
    public string? AuthorsProg { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--merge")]
    public virtual bool? Merge { get; set; }

    [CliOption("--strategy", Format = OptionFormat.EqualsSeparated)]
    public string? Strategy { get; set; }

    [CliFlag("--rebase-merges")]
    public virtual bool? RebaseMerges { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("--use-log-author")]
    public virtual bool? UseLogAuthor { get; set; }

    [CliFlag("--add-author-from")]
    public virtual bool? AddAuthorFrom { get; set; }

    [CliOption("--id", Format = OptionFormat.EqualsSeparated)]
    public string? Id { get; set; }

    [CliOption("--svn-remote", Format = OptionFormat.EqualsSeparated)]
    public string? SvnRemote { get; set; }

    [CliFlag("--follow-parent")]
    public virtual bool? FollowParent { get; set; }
}