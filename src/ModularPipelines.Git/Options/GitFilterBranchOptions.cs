using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("filter-branch")]
[ExcludeFromCodeCoverage]
public record GitFilterBranchOptions : GitOptions
{
    [CliOption("--setup", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Setup { get; set; }

    [CliOption("--subdirectory-filter", Format = OptionFormat.EqualsSeparated)]
    public virtual string? SubdirectoryFilter { get; set; }

    [CliOption("--env-filter", Format = OptionFormat.EqualsSeparated)]
    public virtual string? EnvFilter { get; set; }

    [CliOption("--tree-filter", Format = OptionFormat.EqualsSeparated)]
    public virtual string? TreeFilter { get; set; }

    [CliOption("--index-filter", Format = OptionFormat.EqualsSeparated)]
    public virtual string? IndexFilter { get; set; }

    [CliOption("--parent-filter", Format = OptionFormat.EqualsSeparated)]
    public virtual string? ParentFilter { get; set; }

    [CliOption("--msg-filter", Format = OptionFormat.EqualsSeparated)]
    public virtual string? MsgFilter { get; set; }

    [CliOption("--commit-filter", Format = OptionFormat.EqualsSeparated)]
    public virtual string? CommitFilter { get; set; }

    [CliOption("--tag-name-filter", Format = OptionFormat.EqualsSeparated)]
    public virtual string? TagNameFilter { get; set; }

    [CliFlag("--prune-empty")]
    public virtual bool? PruneEmpty { get; set; }

    [CliOption("--original", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Original { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliOption("--state-branch", Format = OptionFormat.EqualsSeparated)]
    public virtual string? StateBranch { get; set; }
}