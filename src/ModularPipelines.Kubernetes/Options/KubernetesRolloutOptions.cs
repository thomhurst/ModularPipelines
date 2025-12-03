using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("rollout")]
[ExcludeFromCodeCoverage]
public record KubernetesRolloutOptions([property: CliArgument] string Subcommand) : KubernetesOptions
{
    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--filename")]
    public string[]? Filename { get; set; }

    [CliOption("--kustomize")]
    public string? Kustomize { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliOption("--revision")]
    public int? Revision { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public string? Template { get; set; }
}