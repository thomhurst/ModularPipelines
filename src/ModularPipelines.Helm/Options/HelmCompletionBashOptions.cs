using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("completion", "bash")]
[ExcludeFromCodeCoverage]
public record HelmCompletionBashOptions : HelmOptions
{
    [CliFlag("--no-descriptions")]
    public virtual bool? NoDescriptions { get; set; }
}