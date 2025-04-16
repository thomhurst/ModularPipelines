using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("gc")]
[ExcludeFromCodeCoverage]
public record GitGcOptions : GitOptions
{
    [BooleanCommandSwitch("--aggressive")]
    public virtual bool? Aggressive { get; set; }

    [BooleanCommandSwitch("--auto")]
    public virtual bool? Auto { get; set; }

    [BooleanCommandSwitch("--no-cruft")]
    public virtual bool? NoCruft { get; set; }

    [BooleanCommandSwitch("--cruft")]
    public virtual bool? Cruft { get; set; }

    [CommandEqualsSeparatorSwitch("--prune")]
    public string? Prune { get; set; }

    [BooleanCommandSwitch("--no-prune")]
    public virtual bool? NoPrune { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--keep-largest-pack")]
    public virtual bool? KeepLargestPack { get; set; }
}