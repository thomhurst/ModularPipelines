using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("read-tree")]
public record GitReadTreeOptions : GitOptions
{
    [BooleanCommandSwitch("--reset")]
    public bool? Reset { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--trivial")]
    public bool? Trivial { get; set; }

    [BooleanCommandSwitch("--aggressive")]
    public bool? Aggressive { get; set; }

    [CommandEqualsSeparatorSwitch("--prefix")]
    public string? Prefix { get; set; }

    [CommandEqualsSeparatorSwitch("--index-output")]
    public string? IndexOutput { get; set; }

    [BooleanCommandSwitch("--no-recurse-submodules")]
    public bool? NoRecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--recurse-submodules")]
    public bool? RecurseSubmodules { get; set; }

    [BooleanCommandSwitch("--no-sparse-checkout")]
    public bool? NoSparseCheckout { get; set; }

    [BooleanCommandSwitch("--empty")]
    public bool? Empty { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }
}