using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("config", "view")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigViewOptions : KubernetesOptions
{
    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliFlag("--flatten")]
    public virtual bool? Flatten { get; set; }

    [CliFlag("--merge")]
    public virtual bool? Merge { get; set; }

    [CliFlag("--minify")]
    public virtual bool? Minify { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliFlag("--raw")]
    public virtual bool? Raw { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public string? Template { get; set; }
}