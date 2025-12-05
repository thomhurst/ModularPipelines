using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("completion", "zsh")]
[ExcludeFromCodeCoverage]
public record HelmCompletionZshOptions : HelmOptions
{
    [CliFlag("--no-descriptions")]
    public virtual bool? NoDescriptions { get; set; }
}