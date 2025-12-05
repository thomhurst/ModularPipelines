using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("config", "rename-context")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigRenameContextOptions([property: CliArgument] string ContextName) : KubernetesOptions
{
    [CliFlag("--set-raw-bytes")]
    public virtual bool? SetRawBytes { get; set; }
}