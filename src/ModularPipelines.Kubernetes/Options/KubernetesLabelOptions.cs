using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliSubCommand("label")]
[ExcludeFromCodeCoverage]
public record KubernetesLabelOptions : KubernetesOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--all-namespaces")]
    public virtual bool? AllNamespaces { get; set; }

    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--dry-run")]
    public virtual string? DryRun { get; set; }

    [CliOption("--field-manager")]
    public virtual string? FieldManager { get; set; }

    [CliOption("--field-selector")]
    public virtual string? FieldSelector { get; set; }

    [CliOption("--filename")]
    public virtual string[]? Filename { get; set; }

    [CliOption("--kustomize")]
    public virtual string? Kustomize { get; set; }

    [CliFlag("--list")]
    public virtual bool? List { get; set; }

    [CliFlag("--local")]
    public virtual bool? Local { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliFlag("--overwrite")]
    public virtual bool? Overwrite { get; set; }

    [CliFlag("--record")]
    public virtual bool? Record { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliOption("--resource-version")]
    public virtual string? ResourceVersion { get; set; }

    [CliOption("--selector")]
    public virtual string? Selector { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public virtual string? Template { get; set; }
}