using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("submodule")]
[ExcludeFromCodeCoverage]
public record GitSubmoduleOptions : GitOptions
{
    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--progress")]
    public virtual bool? Progress { get; set; }

    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [CommandEqualsSeparatorSwitch("--branch")]
    public string? Branch { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--cached")]
    public virtual bool? Cached { get; set; }

    [BooleanCommandSwitch("--files")]
    public virtual bool? Files { get; set; }

    [BooleanCommandSwitch("--summary-limit")]
    public virtual bool? SummaryLimit { get; set; }

    [BooleanCommandSwitch("--remote")]
    public virtual bool? Remote { get; set; }

    [BooleanCommandSwitch("--no-fetch")]
    public virtual bool? NoFetch { get; set; }

    [BooleanCommandSwitch("--checkout")]
    public virtual bool? Checkout { get; set; }

    [BooleanCommandSwitch("--merge")]
    public virtual bool? Merge { get; set; }

    [BooleanCommandSwitch("--rebase")]
    public virtual bool? Rebase { get; set; }

    [BooleanCommandSwitch("--init")]
    public virtual bool? Init { get; set; }

    [BooleanCommandSwitch("--name")]
    public virtual bool? Name { get; set; }

    [CommandEqualsSeparatorSwitch("--reference")]
    public string? Reference { get; set; }

    [BooleanCommandSwitch("--dissociate")]
    public virtual bool? Dissociate { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public virtual bool? Recursive { get; set; }

    [BooleanCommandSwitch("--depth")]
    public virtual bool? Depth { get; set; }

    [BooleanCommandSwitch("--no-recommend-shallow")]
    public virtual bool? NoRecommendShallow { get; set; }

    [BooleanCommandSwitch("--recommend-shallow")]
    public virtual bool? RecommendShallow { get; set; }

    [CommandEqualsSeparatorSwitch("--jobs")]
    public string? Jobs { get; set; }

    [BooleanCommandSwitch("--no-single-branch")]
    public virtual bool? NoSingleBranch { get; set; }

    [BooleanCommandSwitch("--single-branch")]
    public virtual bool? SingleBranch { get; set; }
}