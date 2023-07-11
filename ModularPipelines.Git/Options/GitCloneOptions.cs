using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("clone")]
public record GitCloneOptions : GitOptions
{
    [BooleanCommandSwitch("--local")]
    public bool? Local { get; set; }

    [BooleanCommandSwitch("--no-hardlinks")]
    public bool? NoHardlinks { get; set; }

    [BooleanCommandSwitch("--shared")]
    public bool? Shared { get; set; }

    [CommandEqualsSeparatorSwitch("--reference-if-able")]
    public string? ReferenceIfAble { get; set; }

    [CommandEqualsSeparatorSwitch("--reference")]
    public string? Reference { get; set; }

    [BooleanCommandSwitch("--dissociate")]
    public bool? Dissociate { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("--progress")]
    public bool? Progress { get; set; }

    [CommandEqualsSeparatorSwitch("--server-option")]
    public string? ServerOption { get; set; }

    [BooleanCommandSwitch("--no-checkout")]
    public bool? NoCheckout { get; set; }

    [BooleanCommandSwitch("--no-reject-shallow")]
    public bool? NoRejectShallow { get; set; }

    [BooleanCommandSwitch("--reject-shallow")]
    public bool? RejectShallow { get; set; }

    [BooleanCommandSwitch("--bare")]
    public bool? Bare { get; set; }

    [BooleanCommandSwitch("--sparse")]
    public bool? Sparse { get; set; }

    [CommandEqualsSeparatorSwitch("--filter")]
    public string? Filter { get; set; }

    [BooleanCommandSwitch("--also-filter-submodules")]
    public bool? AlsoFilterSubmodules { get; set; }

    [BooleanCommandSwitch("--mirror")]
    public bool? Mirror { get; set; }

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
    public bool? NoSingleBranch { get; set; }

    [BooleanCommandSwitch("--single-branch")]
    public bool? SingleBranch { get; set; }

    [BooleanCommandSwitch("--no-tags")]
    public bool? NoTags { get; set; }

    [CommandEqualsSeparatorSwitch("--recurse-submodules")]
    public string? RecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--no-shallow-submodules")]
    public bool? NoShallowSubmodules { get; set; }

    [BooleanCommandSwitch("--shallow-submodules")]
    public bool? ShallowSubmodules { get; set; }

    [BooleanCommandSwitch("--no-remote-submodules")]
    public bool? NoRemoteSubmodules { get; set; }

    [BooleanCommandSwitch("--remote-submodules")]
    public bool? RemoteSubmodules { get; set; }

    [CommandEqualsSeparatorSwitch("--separate-git-dir")]
    public string? SeparateGitDir { get; set; }

    [CommandEqualsSeparatorSwitch("--jobs")]
    public string? Jobs { get; set; }

    [CommandEqualsSeparatorSwitch("--bundle-uri")]
    public string? BundleUri { get; set; }

}