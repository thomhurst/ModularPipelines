using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesCreateSecretDockerRegistryOptions : KubernetesOptions
{
    public KubernetesCreateSecretDockerRegistryOptions(
        string dockerUsername,
        string dockerPassword,
        string dockerEmail
)
    {
        CommandParts = ["create", "secret", "docker-registry"];
        DockerUsername = dockerUsername;
        DockerPassword = dockerPassword;
        DockerEmail = dockerEmail;
    }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [BooleanCommandSwitch("--append-hash")]
    public bool? AppendHash { get; set; }

    [CommandSwitch("--docker-email")]
    public string? DockerEmail { get; set; }

    [CommandSwitch("--docker-password")]
    public string? DockerPassword { get; set; }

    [CommandSwitch("--docker-server")]
    public string? DockerServer { get; set; }

    [CommandSwitch("--docker-username")]
    public string? DockerUsername { get; set; }

    [CommandSwitch("--dry-run")]
    public string? DryRun { get; set; }

    [CommandSwitch("--field-manager")]
    public string? FieldManager { get; set; }

    [CommandSwitch("--from-file")]
    public IEnumerable<string>? FromFile { get; set; }

    [PositionalArgument(PlaceholderName = "<NAME>")]
    public string? NAME { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--save-config")]
    public bool? SaveConfig { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }
}
