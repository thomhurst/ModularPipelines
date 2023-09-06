using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("completion", "powershell")]
[ExcludeFromCodeCoverage]
public record HelmCompletionPowershellOptions : HelmOptions
{
    [BooleanCommandSwitch("--no-descriptions")]
    public bool? NoDescriptions { get; set; }
}
