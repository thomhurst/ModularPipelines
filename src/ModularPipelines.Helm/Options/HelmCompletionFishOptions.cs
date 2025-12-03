using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("completion", "fish")]
[ExcludeFromCodeCoverage]
public record HelmCompletionFishOptions : HelmOptions
{
    [CliFlag("--no-descriptions")]
    public virtual bool? NoDescriptions { get; set; }
}