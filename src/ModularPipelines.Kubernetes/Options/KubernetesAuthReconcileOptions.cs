using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("auth", "reconcile")]
[ExcludeFromCodeCoverage]
public record KubernetesAuthReconcileOptions : KubernetesOptions
{
    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--dry-run")]
    public string? DryRun { get; set; }

    [CliOption("--filename")]
    public string[]? Filename { get; set; }

    [CliOption("--kustomize")]
    public string? Kustomize { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliFlag("--remove-extra-permissions")]
    public virtual bool? RemoveExtraPermissions { get; set; }

    [CliFlag("--remove-extra-subjects")]
    public virtual bool? RemoveExtraSubjects { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public string? Template { get; set; }
}