using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("bundle")]
[ExcludeFromCodeCoverage]
public record GitBundleOptions : GitOptions
{
    [CliFlag("--progress")]
    public virtual bool? Progress { get; set; }

    [CliOption("--version", Format = OptionFormat.EqualsSeparated)]
    public virtual string? Version { get; set; }

    [CliFlag("--quiet")]
    public virtual bool? Quiet { get; set; }
}