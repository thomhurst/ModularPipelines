using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("create", "priorityclass")]
[ExcludeFromCodeCoverage]
public record KubernetesCreatePriorityClassOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--dry-run")]
    public string? DryRun { get; set; }

    [CliOption("--field-manager")]
    public string? FieldManager { get; set; }

    [CliFlag("--global-default")]
    public virtual bool? GlobalDefault { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliOption("--preemption-policy")]
    public string? PreemptionPolicy { get; set; }

    [CliFlag("--save-config")]
    public virtual bool? SaveConfig { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public string? Template { get; set; }

    [CliFlag("--validate")]
    public virtual bool? Validate { get; set; }

    [CliOption("--value")]
    public int? Value { get; set; }
}