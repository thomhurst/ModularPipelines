using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliSubCommand("explain")]
[ExcludeFromCodeCoverage]
public record KubernetesExplainOptions([property: CliArgument] string Resource) : KubernetesOptions
{
    [CliOption("--api-version")]
    public virtual string? ApiVersion { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }
}