using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("clone")]
[ExcludeFromCodeCoverage]
public record GitCloneOptions : GitOptions
{
    [BooleanCommandSwitch("--local")]
    public virtual bool? Local { get; set; }

    [BooleanCommandSwitch("--no-hardlinks")]
    public virtual bool? NoHardlinks { get; set; }

    [BooleanCommandSwitch("--shared")]
    public virtual bool? Shared { get; set; }

    [CommandEqualsSeparatorSwitch("--reference-if-able")]
    public string? ReferenceIfAble { get; set; }

    [CommandEqualsSeparatorSwitch("--reference")]
    public string? Reference { get; set; }

    [BooleanCommandSwitch("--dissociate")]
    public virtual bool? Dissociate { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public virtual bool? Verbose { get; set; }

    [BooleanCommandSwitch("--progress")]
    public virtual bool? Progress { get; set; }

    [CommandEqualsSeparatorSwitch("--server-option")]
    public string? ServerOption { get; set; }

    [BooleanCommandSwitch("--no-checkout")]
    public virtual bool? NoCheckout { get; set; }

    [BooleanCommandSwitch("--no-reject-shallow")]
    public virtual bool? NoRejectShallow { get; set; }

    [BooleanCommandSwitch("--reject-shallow")]
    public virtual bool? RejectShallow { get; set; }

    [BooleanCommandSwitch("--bare")]
    public virtual bool? Bare { get; set; }

    [BooleanCommandSwitch("--sparse")]
    public virtual bool? Sparse { get; set; }

    [CommandEqualsSeparatorSwitch("--filter")]
    public string? Filter { get; set; }

    [BooleanCommandSwitch("--also-filter-submodules")]
    public virtual bool? AlsoFilterSubmodules { get; set; }

    [BooleanCommandSwitch("--mirror")]
    public virtual bool? Mirror { get; set; }

    [CommandEqualsSeparatorSwitch("--origin")]
    public string? Origin { get; set; }

    [CommandEqualsSeparatorSwitch("--branch")]
    public string? Branch { get; set; }

    [CommandEqualsSeparatorSwitch("--upload-pack")]
    public string? UploadPack { get; set; }

    [CommandEqualsSeparatorSwitch("--template")]
    public string? Template { get; set; }

    [CommandEqualsSeparatorSwitch("--config")]
    public string? Config { get; set; }

    [CommandEqualsSeparatorSwitch("--depth")]
    public string? Depth { get; set; }

    [CommandEqualsSeparatorSwitch("--shallow-since")]
    public string? ShallowSince { get; set; }

    [CommandEqualsSeparatorSwitch("--shallow-exclude")]
    public string? ShallowExclude { get; set; }

    [BooleanCommandSwitch("--no-single-branch")]
    public virtual bool? NoSingleBranch { get; set; }

    [BooleanCommandSwitch("--single-branch")]
    public virtual bool? SingleBranch { get; set; }

    [BooleanCommandSwitch("--no-tags")]
    public virtual bool? NoTags { get; set; }

    [CommandEqualsSeparatorSwitch("--recurse-submodules")]
    public string? RecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--no-shallow-submodules")]
    public virtual bool? NoShallowSubmodules { get; set; }

    [BooleanCommandSwitch("--shallow-submodules")]
    public virtual bool? ShallowSubmodules { get; set; }

    [BooleanCommandSwitch("--no-remote-submodules")]
    public virtual bool? NoRemoteSubmodules { get; set; }

    [BooleanCommandSwitch("--remote-submodules")]
    public virtual bool? RemoteSubmodules { get; set; }

    [CommandEqualsSeparatorSwitch("--separate-git-dir")]
    public string? SeparateGitDir { get; set; }

    [CommandEqualsSeparatorSwitch("--jobs")]
    public string? Jobs { get; set; }

    [CommandEqualsSeparatorSwitch("--bundle-uri")]
    public string? BundleUri { get; set; }
}