using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliCommand("read-tree")]
[ExcludeFromCodeCoverage]
public record GitReadTreeOptions : GitOptions
{
    [CliFlag("--reset")]
    public virtual bool? Reset { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("--trivial")]
    public virtual bool? Trivial { get; set; }

    [CliFlag("--aggressive")]
    public virtual bool? Aggressive { get; set; }

    [CliOption("--prefix", Format = OptionFormat.EqualsSeparated)]
    public string? Prefix { get; set; }

    [CliOption("--index-output", Format = OptionFormat.EqualsSeparated)]
    public string? IndexOutput { get; set; }

    [CliFlag("--no-recurse-submodules")]
    public virtual bool? NoRecurseSubmodules { get; set; }

    [CliFlag("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [CliFlag("--no-sparse-checkout")]
    public virtual bool? NoSparseCheckout { get; set; }

    [CliFlag("--empty")]
    public virtual bool? Empty { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}