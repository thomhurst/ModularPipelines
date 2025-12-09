using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliSubCommand("rollout")]
[ExcludeFromCodeCoverage]
public record KubernetesRolloutOptions([property: CliArgument] string Subcommand) : KubernetesOptions
{
    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--filename")]
    public virtual string[]? Filename { get; set; }

    [CliOption("--kustomize")]
    public virtual string? Kustomize { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliOption("--revision")]
    public virtual int? Revision { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public virtual string? Template { get; set; }
}