using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("clone")]
[ExcludeFromCodeCoverage]
public record GitCloneOptions : GitOptions
{
    [CliFlag("--local")]
    public virtual bool? Local { get; set; }

    [CliFlag("--no-hardlinks")]
    public virtual bool? NoHardlinks { get; set; }

    [CliFlag("--shared")]
    public virtual bool? Shared { get; set; }

    [CliOption("--reference-if-able", Format = OptionFormat.EqualsSeparated)]
    public string? ReferenceIfAble { get; set; }

    [CliOption("--reference", Format = OptionFormat.EqualsSeparated)]
    public string? Reference { get; set; }

    [CliFlag("--dissociate")]
    public virtual bool? Dissociate { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--verbose")]
    public virtual bool? Verbose { get; set; }

    [CliFlag("--progress")]
    public virtual bool? Progress { get; set; }

    [CliOption("--server-option", Format = OptionFormat.EqualsSeparated)]
    public string? ServerOption { get; set; }

    [CliFlag("--no-checkout")]
    public virtual bool? NoCheckout { get; set; }

    [CliFlag("--no-reject-shallow")]
    public virtual bool? NoRejectShallow { get; set; }

    [CliFlag("--reject-shallow")]
    public virtual bool? RejectShallow { get; set; }

    [CliFlag("--bare")]
    public virtual bool? Bare { get; set; }

    [CliFlag("--sparse")]
    public virtual bool? Sparse { get; set; }

    [CliOption("--filter", Format = OptionFormat.EqualsSeparated)]
    public string? Filter { get; set; }

    [CliFlag("--also-filter-submodules")]
    public virtual bool? AlsoFilterSubmodules { get; set; }

    [CliFlag("--mirror")]
    public virtual bool? Mirror { get; set; }

    [CliOption("--origin", Format = OptionFormat.EqualsSeparated)]
    public string? Origin { get; set; }

    [CliOption("--branch", Format = OptionFormat.EqualsSeparated)]
    public string? Branch { get; set; }

    [CliOption("--upload-pack", Format = OptionFormat.EqualsSeparated)]
    public string? UploadPack { get; set; }

    [CliOption("--template", Format = OptionFormat.EqualsSeparated)]
    public string? Template { get; set; }

    [CliOption("--config", Format = OptionFormat.EqualsSeparated)]
    public string? Config { get; set; }

    [CliOption("--depth", Format = OptionFormat.EqualsSeparated)]
    public string? Depth { get; set; }

    [CliOption("--shallow-since", Format = OptionFormat.EqualsSeparated)]
    public string? ShallowSince { get; set; }

    [CliOption("--shallow-exclude", Format = OptionFormat.EqualsSeparated)]
    public string? ShallowExclude { get; set; }

    [CliFlag("--no-single-branch")]
    public virtual bool? NoSingleBranch { get; set; }

    [CliFlag("--single-branch")]
    public virtual bool? SingleBranch { get; set; }

    [CliFlag("--no-tags")]
    public virtual bool? NoTags { get; set; }

    [CliOption("--recurse-submodules", Format = OptionFormat.EqualsSeparated)]
    public string? RecurseSubmodules { get; set; }

    [CliFlag("--no-shallow-submodules")]
    public virtual bool? NoShallowSubmodules { get; set; }

    [CliFlag("--shallow-submodules")]
    public virtual bool? ShallowSubmodules { get; set; }

    [CliFlag("--no-remote-submodules")]
    public virtual bool? NoRemoteSubmodules { get; set; }

    [CliFlag("--remote-submodules")]
    public virtual bool? RemoteSubmodules { get; set; }

    [CliOption("--separate-git-dir", Format = OptionFormat.EqualsSeparated)]
    public string? SeparateGitDir { get; set; }

    [CliOption("--jobs", Format = OptionFormat.EqualsSeparated)]
    public string? Jobs { get; set; }

    [CliOption("--bundle-uri", Format = OptionFormat.EqualsSeparated)]
    public string? BundleUri { get; set; }
}