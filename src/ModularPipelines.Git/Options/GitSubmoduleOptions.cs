using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("submodule")]
[ExcludeFromCodeCoverage]
public record GitSubmoduleOptions : GitOptions
{
    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--progress")]
    public virtual bool? Progress { get; set; }

    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliOption("--branch", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Branch { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--cached")]
    public virtual bool? Cached { get; set; }

    [CliFlag("--files")]
    public virtual bool? Files { get; set; }

    [CliFlag("--summary-limit")]
    public virtual bool? SummaryLimit { get; set; }

    [CliFlag("--remote")]
    public virtual bool? Remote { get; set; }

    [CliFlag("--no-fetch")]
    public virtual bool? NoFetch { get; set; }

    [CliFlag("--checkout")]
    public virtual bool? Checkout { get; set; }

    [CliFlag("--merge")]
    public virtual bool? Merge { get; set; }

    [CliFlag("--rebase")]
    public virtual bool? Rebase { get; set; }

    [CliFlag("--init")]
    public virtual bool? Init { get; set; }

    [CliFlag("--name")]
    public virtual bool? Name { get; set; }

    [CliOption("--reference", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Reference { get; set; }

    [CliFlag("--dissociate")]
    public virtual bool? Dissociate { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliFlag("--depth")]
    public virtual bool? Depth { get; set; }

    [CliFlag("--no-recommend-shallow")]
    public virtual bool? NoRecommendShallow { get; set; }

    [CliFlag("--recommend-shallow")]
    public virtual bool? RecommendShallow { get; set; }

    [CliOption("--jobs", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Jobs { get; set; }

    [CliFlag("--no-single-branch")]
    public virtual bool? NoSingleBranch { get; set; }

    [CliFlag("--single-branch")]
    public virtual bool? SingleBranch { get; set; }
}