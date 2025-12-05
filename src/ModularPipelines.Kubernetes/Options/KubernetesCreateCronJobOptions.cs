using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("create", "cronjob")]
[ExcludeFromCodeCoverage]
public record KubernetesCreateCronJobOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--dry-run")]
    public virtual string? DryRun { get; set; }

    [CliOption("--field-manager")]
    public virtual string? FieldManager { get; set; }

    [CliOption("--image")]
    public virtual string? Image { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliOption("--restart")]
    public virtual string? Restart { get; set; }

    [CliFlag("--save-config")]
    public virtual bool? SaveConfig { get; set; }

    [CliOption("--schedule")]
    public virtual string? Schedule { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public virtual string? Template { get; set; }

    [CliFlag("--validate")]
    public virtual bool? Validate { get; set; }
}