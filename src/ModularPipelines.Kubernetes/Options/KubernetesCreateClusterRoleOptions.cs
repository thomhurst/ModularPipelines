using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("create", "clusterrole")]
[ExcludeFromCodeCoverage]
public record KubernetesCreateClusterRoleOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliOption("--aggregation-rule")]
    public virtual string? AggregationRule { get; set; }

    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--dry-run")]
    public virtual string? DryRun { get; set; }

    [CliOption("--field-manager")]
    public virtual string? FieldManager { get; set; }

    [CliOption("--non-resource-url")]
    public virtual string[]? NonResourceUrl { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliOption("--resource")]
    public virtual string[]? Resource { get; set; }

    [CliOption("--resource-name")]
    public virtual string[]? ResourceName { get; set; }

    [CliFlag("--save-config")]
    public virtual bool? SaveConfig { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public virtual string? Template { get; set; }

    [CliFlag("--validate")]
    public virtual bool? Validate { get; set; }

    [CliOption("--verb")]
    public virtual string[]? Verb { get; set; }
}