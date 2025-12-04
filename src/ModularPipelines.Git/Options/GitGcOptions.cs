using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("gc")]
[ExcludeFromCodeCoverage]
public record GitGcOptions : GitOptions
{
    [CliFlag("--aggressive")]
    public virtual bool? Aggressive { get; set; }

    [CliFlag("--auto")]
    public virtual bool? Auto { get; set; }

    [CliFlag("--no-cruft")]
    public virtual bool? NoCruft { get; set; }

    [CliFlag("--cruft")]
    public virtual bool? Cruft { get; set; }

    [CliOption("--prune", Format = OptionFormat.EqualsSeparated)]
    public string? Prune { get; set; }

    [CliFlag("--no-prune")]
    public virtual bool? NoPrune { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--keep-largest-pack")]
    public virtual bool? KeepLargestPack { get; set; }
}