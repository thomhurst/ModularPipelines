using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("submodule")]
[ExcludeFromCodeCoverage]
public record GitSubmoduleOptions : GitOptions
{
    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [BooleanCommandSwitch("--progress")]
    public bool? Progress { get; set; }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandEqualsSeparatorSwitch("--branch")]
    public string? Branch { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--cached")]
    public bool? Cached { get; set; }

    [BooleanCommandSwitch("--files")]
    public bool? Files { get; set; }

    [BooleanCommandSwitch("--summary-limit")]
    public bool? SummaryLimit { get; set; }

    [BooleanCommandSwitch("--remote")]
    public bool? Remote { get; set; }

    [BooleanCommandSwitch("--no-fetch")]
    public bool? NoFetch { get; set; }

    [BooleanCommandSwitch("--checkout")]
    public bool? Checkout { get; set; }

    [BooleanCommandSwitch("--merge")]
    public bool? Merge { get; set; }

    [BooleanCommandSwitch("--rebase")]
    public bool? Rebase { get; set; }

    [BooleanCommandSwitch("--init")]
    public bool? Init { get; set; }

    [BooleanCommandSwitch("--name")]
    public bool? Name { get; set; }

    [CommandEqualsSeparatorSwitch("--reference")]
    public string? Reference { get; set; }

    [BooleanCommandSwitch("--dissociate")]
    public bool? Dissociate { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [BooleanCommandSwitch("--depth")]
    public bool? Depth { get; set; }

    [BooleanCommandSwitch("--no-recommend-shallow")]
    public bool? NoRecommendShallow { get; set; }

    [BooleanCommandSwitch("--recommend-shallow")]
    public bool? RecommendShallow { get; set; }

    [CommandEqualsSeparatorSwitch("--jobs")]
    public string? Jobs { get; set; }

    [BooleanCommandSwitch("--no-single-branch")]
    public bool? NoSingleBranch { get; set; }

    [BooleanCommandSwitch("--single-branch")]
    public bool? SingleBranch { get; set; }
}