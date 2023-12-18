using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Git.Options;

[CommandPrecedingArguments("merge-base")]
[ExcludeFromCodeCoverage]
public record GitMergeBaseOptions : GitOptions
{
    [BooleanCommandSwitch("--octopus")]
    public bool? Octopus { get; set; }

    [BooleanCommandSwitch("--independent")]
    public bool? Independent { get; set; }

    [BooleanCommandSwitch("--is-ancestor")]
    public bool? IsAncestor { get; set; }

    [BooleanCommandSwitch("--fork-point")]
    public bool? ForkPoint { get; set; }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }
}