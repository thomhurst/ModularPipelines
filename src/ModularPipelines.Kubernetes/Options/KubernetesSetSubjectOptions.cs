using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("set", "subject")]
[ExcludeFromCodeCoverage]
public record KubernetesSetSubjectOptions : KubernetesOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--dry-run")]
    public string? DryRun { get; set; }

    [CliOption("--field-manager")]
    public string? FieldManager { get; set; }

    [CliOption("--filename")]
    public string[]? Filename { get; set; }

    [CliOption("--group")]
    public string[]? Group { get; set; }

    [CliOption("--kustomize")]
    public string? Kustomize { get; set; }

    [CliFlag("--local")]
    public virtual bool? Local { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliOption("--selector")]
    public string? Selector { get; set; }

    [CliOption("--serviceaccount")]
    public string[]? Serviceaccount { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public string? Template { get; set; }
}