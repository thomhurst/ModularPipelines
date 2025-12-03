using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("filter-branch")]
[ExcludeFromCodeCoverage]
public record GitFilterBranchOptions : GitOptions
{
    [CliOption("--setup", Format = OptionFormat.EqualsSeparated)]
    public string? Setup { get; set; }

    [CliOption("--subdirectory-filter", Format = OptionFormat.EqualsSeparated)]
    public string? SubdirectoryFilter { get; set; }

    [CliOption("--env-filter", Format = OptionFormat.EqualsSeparated)]
    public string? EnvFilter { get; set; }

    [CliOption("--tree-filter", Format = OptionFormat.EqualsSeparated)]
    public string? TreeFilter { get; set; }

    [CliOption("--index-filter", Format = OptionFormat.EqualsSeparated)]
    public string? IndexFilter { get; set; }

    [CliOption("--parent-filter", Format = OptionFormat.EqualsSeparated)]
    public string? ParentFilter { get; set; }

    [CliOption("--msg-filter", Format = OptionFormat.EqualsSeparated)]
    public string? MsgFilter { get; set; }

    [CliOption("--commit-filter", Format = OptionFormat.EqualsSeparated)]
    public string? CommitFilter { get; set; }

    [CliOption("--tag-name-filter", Format = OptionFormat.EqualsSeparated)]
    public string? TagNameFilter { get; set; }

    [CliFlag("--prune-empty")]
    public virtual bool? PruneEmpty { get; set; }

    [CliOption("--original", Format = OptionFormat.EqualsSeparated)]
    public string? Original { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliOption("--state-branch", Format = OptionFormat.EqualsSeparated)]
    public string? StateBranch { get; set; }
}