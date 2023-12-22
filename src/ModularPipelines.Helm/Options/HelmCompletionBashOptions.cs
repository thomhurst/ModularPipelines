using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("completion", "bash")]
[ExcludeFromCodeCoverage]
public record HelmCompletionBashOptions : HelmOptions
{
    [BooleanCommandSwitch("--no-descriptions")]
    public bool? NoDescriptions { get; set; }
}