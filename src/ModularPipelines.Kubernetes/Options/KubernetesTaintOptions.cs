using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("taint")]
[ExcludeFromCodeCoverage]
public record KubernetesTaintOptions([property: CliArgument] string Node) : KubernetesOptions
{
    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--dry-run")]
    public virtual string? DryRun { get; set; }

    [CliOption("--field-manager")]
    public virtual string? FieldManager { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliFlag("--overwrite")]
    public virtual bool? Overwrite { get; set; }

    [CliOption("--selector")]
    public virtual string? Selector { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public virtual string? Template { get; set; }

    [CliFlag("--validate")]
    public virtual bool? Validate { get; set; }
}