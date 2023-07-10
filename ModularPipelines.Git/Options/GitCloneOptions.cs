using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("clone")]
public record GitCloneOptions : GitOptions
{
    [BooleanCommandSwitch("local")]
    public bool? Local { get; set; }

    [BooleanCommandSwitch("no-hardlinks")]
    public bool? NoHardlinks { get; set; }

    [BooleanCommandSwitch("shared")]
    public bool? Shared { get; set; }

    [CommandLongSwitch("reference-if-able")]
    public string? ReferenceIfAble { get; set; }

    [CommandLongSwitch("reference")]
    public string? Reference { get; set; }

    [BooleanCommandSwitch("dissociate")]
    public bool? Dissociate { get; set; }

    [BooleanCommandSwitch("quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("verbose")]
    public bool? Verbose { get; set; }

    [BooleanCommandSwitch("progress")]
    public bool? Progress { get; set; }

    [CommandLongSwitch("server-option")]
    public string? ServerOption { get; set; }

    [BooleanCommandSwitch("no-checkout")]
    public bool? NoCheckout { get; set; }

    [BooleanCommandSwitch("no-reject-shallow")]
    public bool? NoRejectShallow { get; set; }

    [BooleanCommandSwitch("reject-shallow")]
    public bool? RejectShallow { get; set; }

    [BooleanCommandSwitch("bare")]
    public bool? Bare { get; set; }

    [BooleanCommandSwitch("sparse")]
    public bool? Sparse { get; set; }

    [CommandLongSwitch("filter")]
    public string? Filter { get; set; }

    [BooleanCommandSwitch("also-filter-submodules")]
    public bool? AlsoFilterSubmodules { get; set; }

    [BooleanCommandSwitch("mirror")]
    public bool? Mirror { get; set; }

    [CommandLongSwitch("origin")]
    public string? Origin { get; set; }

    [CommandLongSwitch("branch")]
    public string? Branch { get; set; }

    [CommandLongSwitch("upload-pack")]
    public string? UploadPack { get; set; }

    [CommandLongSwitch("template")]
    public string? Template { get; set; }

    [CommandLongSwitch("config")]
    public string? Config { get; set; }

    [CommandLongSwitch("depth")]
    public string? Depth { get; set; }

    [CommandLongSwitch("shallow-since")]
    public string? ShallowSince { get; set; }

    [CommandLongSwitch("shallow-exclude")]
    public string? ShallowExclude { get; set; }

    [BooleanCommandSwitch("no-single-branch")]
    public bool? NoSingleBranch { get; set; }

    [BooleanCommandSwitch("single-branch")]
    public bool? SingleBranch { get; set; }

    [BooleanCommandSwitch("no-tags")]
    public bool? NoTags { get; set; }

    [CommandLongSwitch("recurse-submodules")]
    public string? RecurseSubmodules { get; set; }

    [BooleanCommandSwitch("no-shallow-submodules")]
    public bool? NoShallowSubmodules { get; set; }

    [BooleanCommandSwitch("shallow-submodules")]
    public bool? ShallowSubmodules { get; set; }

    [BooleanCommandSwitch("no-remote-submodules")]
    public bool? NoRemoteSubmodules { get; set; }

    [BooleanCommandSwitch("remote-submodules")]
    public bool? RemoteSubmodules { get; set; }

    [CommandLongSwitch("separate-git-dir")]
    public string? SeparateGitDir { get; set; }

    [CommandLongSwitch("jobs")]
    public string? Jobs { get; set; }

    [CommandLongSwitch("bundle-uri")]
    public string? BundleUri { get; set; }

}