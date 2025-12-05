using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("create", "poddisruptionbudget")]
[ExcludeFromCodeCoverage]
public record KubernetesCreatePodDisruptionBudgetOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--dry-run")]
    public virtual string? DryRun { get; set; }

    [CliOption("--field-manager")]
    public virtual string? FieldManager { get; set; }

    [CliOption("--max-unavailable")]
    public virtual string? MaxUnavailable { get; set; }

    [CliOption("--min-available")]
    public virtual string? MinAvailable { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliFlag("--save-config")]
    public virtual bool? SaveConfig { get; set; }

    [CliOption("--selector")]
    public virtual string? Selector { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public virtual string? Template { get; set; }

    [CliFlag("--validate")]
    public virtual bool? Validate { get; set; }
}