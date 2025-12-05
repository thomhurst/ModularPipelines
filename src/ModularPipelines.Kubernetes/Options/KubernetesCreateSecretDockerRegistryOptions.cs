using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("create", "docker-registry")]
[ExcludeFromCodeCoverage]
public record KubernetesCreateSecretDockerRegistryOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliFlag("--append-hash")]
    public virtual bool? AppendHash { get; set; }

    [CliOption("--docker-email")]
    public virtual string? DockerEmail { get; set; }

    [CliOption("--docker-password")]
    public virtual string? DockerPassword { get; set; }

    [CliOption("--docker-server")]
    public virtual string? DockerServer { get; set; }

    [CliOption("--docker-username")]
    public virtual string? DockerUsername { get; set; }

    [CliOption("--dry-run")]
    public virtual string? DryRun { get; set; }

    [CliOption("--field-manager")]
    public virtual string? FieldManager { get; set; }

    [CliOption("--from-file")]
    public virtual string[]? FromFile { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliFlag("--save-config")]
    public virtual bool? SaveConfig { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public virtual string? Template { get; set; }

    [CliFlag("--validate")]
    public virtual bool? Validate { get; set; }
}