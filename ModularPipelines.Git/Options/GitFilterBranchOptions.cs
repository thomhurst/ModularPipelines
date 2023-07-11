using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("filter-branch")]
public record GitFilterBranchOptions : GitOptions
{
    [CommandEqualsSeparatorSwitch("--setup")]
    public string? Setup { get; set; }

    [CommandEqualsSeparatorSwitch("--subdirectory-filter")]
    public string? SubdirectoryFilter { get; set; }

    [CommandEqualsSeparatorSwitch("--env-filter")]
    public string? EnvFilter { get; set; }

    [CommandEqualsSeparatorSwitch("--tree-filter")]
    public string? TreeFilter { get; set; }

    [CommandEqualsSeparatorSwitch("--index-filter")]
    public string? IndexFilter { get; set; }

    [CommandEqualsSeparatorSwitch("--parent-filter")]
    public string? ParentFilter { get; set; }

    [CommandEqualsSeparatorSwitch("--msg-filter")]
    public string? MsgFilter { get; set; }

    [CommandEqualsSeparatorSwitch("--commit-filter")]
    public string? CommitFilter { get; set; }

    [CommandEqualsSeparatorSwitch("--tag-name-filter")]
    public string? TagNameFilter { get; set; }

    [BooleanCommandSwitch("--prune-empty")]
    public bool? PruneEmpty { get; set; }

    [CommandEqualsSeparatorSwitch("--original")]
    public string? Original { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandEqualsSeparatorSwitch("--state-branch")]
    public string? StateBranch { get; set; }

}