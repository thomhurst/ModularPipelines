using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("create", "deployment")]
[ExcludeFromCodeCoverage]
public record KubernetesCreateDeploymentOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--dry-run")]
    public string? DryRun { get; set; }

    [CliOption("--field-manager")]
    public string? FieldManager { get; set; }

    [CliOption("--image")]
    public string[]? Image { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--replicas")]
    public int? Replicas { get; set; }

    [CliFlag("--save-config")]
    public virtual bool? SaveConfig { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public string? Template { get; set; }

    [CliFlag("--validate")]
    public virtual bool? Validate { get; set; }
}