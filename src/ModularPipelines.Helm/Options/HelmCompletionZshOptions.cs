using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("completion", "zsh")]
[ExcludeFromCodeCoverage]
public record HelmCompletionZshOptions : HelmOptions
{
    [BooleanCommandSwitch("--no-descriptions")]
    public virtual bool? NoDescriptions { get; set; }
}