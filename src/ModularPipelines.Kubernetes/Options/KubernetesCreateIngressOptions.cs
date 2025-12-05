using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("create", "ingress")]
[ExcludeFromCodeCoverage]
public record KubernetesCreateIngressOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--annotation")]
    public virtual string[]? Annotation { get; set; }

    [CliOption("--class")]
    public virtual string? Class { get; set; }

    [CliOption("--default-backend")]
    public virtual string? DefaultBackend { get; set; }

    [CliOption("--dry-run")]
    public virtual string? DryRun { get; set; }

    [CliOption("--field-manager")]
    public virtual string? FieldManager { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliOption("--rule")]
    public virtual string[]? Rule { get; set; }

    [CliFlag("--save-config")]
    public virtual bool? SaveConfig { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public virtual string? Template { get; set; }

    [CliFlag("--validate")]
    public virtual bool? Validate { get; set; }
}