using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("read-tree")]
[ExcludeFromCodeCoverage]
public record GitReadTreeOptions : GitOptions
{
    [BooleanCommandSwitch("--reset")]
    public virtual bool? Reset { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("--trivial")]
    public virtual bool? Trivial { get; set; }

    [BooleanCommandSwitch("--aggressive")]
    public virtual bool? Aggressive { get; set; }

    [CommandEqualsSeparatorSwitch("--prefix")]
    public string? Prefix { get; set; }

    [CommandEqualsSeparatorSwitch("--index-output")]
    public string? IndexOutput { get; set; }

    [BooleanCommandSwitch("--no-recurse-submodules")]
    public virtual bool? NoRecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--recurse-submodules")]
    public virtual bool? RecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--no-sparse-checkout")]
    public virtual bool? NoSparseCheckout { get; set; }

    [BooleanCommandSwitch("--empty")]
    public virtual bool? Empty { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }
}