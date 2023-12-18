using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("completion", "fish")]
[ExcludeFromCodeCoverage]
public record HelmCompletionFishOptions : HelmOptions
{
    [BooleanCommandSwitch("--no-descriptions")]
    public bool? NoDescriptions { get; set; }
}