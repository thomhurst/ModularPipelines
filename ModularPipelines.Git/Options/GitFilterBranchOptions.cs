using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("filter-branch")]
public record GitFilterBranchOptions : GitOptions
{
    [CommandLongSwitch("setup")]
    public string? Setup { get; set; }

    [CommandLongSwitch("subdirectory-filter")]
    public string? SubdirectoryFilter { get; set; }

    [CommandLongSwitch("env-filter")]
    public string? EnvFilter { get; set; }

    [CommandLongSwitch("tree-filter")]
    public string? TreeFilter { get; set; }

    [CommandLongSwitch("index-filter")]
    public string? IndexFilter { get; set; }

    [CommandLongSwitch("parent-filter")]
    public string? ParentFilter { get; set; }

    [CommandLongSwitch("msg-filter")]
    public string? MsgFilter { get; set; }

    [CommandLongSwitch("commit-filter")]
    public string? CommitFilter { get; set; }

    [CommandLongSwitch("tag-name-filter")]
    public string? TagNameFilter { get; set; }

    [BooleanCommandSwitch("prune-empty")]
    public bool? PruneEmpty { get; set; }

    [CommandLongSwitch("original")]
    public string? Original { get; set; }

    [BooleanCommandSwitch("force")]
    public bool? Force { get; set; }

    [CommandLongSwitch("state-branch")]
    public string? StateBranch { get; set; }

}