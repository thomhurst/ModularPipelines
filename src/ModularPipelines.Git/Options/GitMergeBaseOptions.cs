using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("merge-base")]
[ExcludeFromCodeCoverage]
public record GitMergeBaseOptions : GitOptions
{
    [BooleanCommandSwitch("--octopus")]
    public virtual bool? Octopus { get; set; }

    [BooleanCommandSwitch("--independent")]
    public virtual bool? Independent { get; set; }

    [BooleanCommandSwitch("--is-ancestor")]
    public virtual bool? IsAncestor { get; set; }

    [BooleanCommandSwitch("--fork-point")]
    public virtual bool? ForkPoint { get; set; }

    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }
}