using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CliSubCommand("merge-base")]
[ExcludeFromCodeCoverage]
public record GitMergeBaseOptions : GitOptions
{
    [CliFlag("--octopus")]
    public virtual bool? Octopus { get; set; }

    [CliFlag("--independent")]
    public virtual bool? Independent { get; set; }

    [CliFlag("--is-ancestor")]
    public virtual bool? IsAncestor { get; set; }

    [CliFlag("--fork-point")]
    public virtual bool? ForkPoint { get; set; }

    [CliFlag("--all")]
    public virtual bool? All { get; set; }
}