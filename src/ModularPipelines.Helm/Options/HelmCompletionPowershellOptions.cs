using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("completion", "powershell")]
[ExcludeFromCodeCoverage]
public record HelmCompletionPowershellOptions : HelmOptions
{
    [CliFlag("--no-descriptions")]
    public virtual bool? NoDescriptions { get; set; }
}