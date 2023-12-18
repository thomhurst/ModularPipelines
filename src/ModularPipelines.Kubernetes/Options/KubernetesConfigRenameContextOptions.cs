using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("config", "rename-context")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigRenameContextOptions([property: PositionalArgument] string ContextName) : KubernetesOptions
{
    [BooleanCommandSwitch("--set-raw-bytes")]
    public bool? SetRawBytes { get; set; }
}