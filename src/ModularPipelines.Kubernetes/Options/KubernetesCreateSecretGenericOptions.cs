using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("create", "generic")]
[ExcludeFromCodeCoverage]
public record KubernetesCreateSecretGenericOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliFlag("--append-hash")]
    public virtual bool? AppendHash { get; set; }

    [CliOption("--dry-run")]
    public virtual string? DryRun { get; set; }

    [CliOption("--field-manager")]
    public virtual string? FieldManager { get; set; }

    [CliOption("--from-env-file")]
    public virtual string? FromEnvFile { get; set; }

    [CliOption("--from-file")]
    public virtual string[]? FromFile { get; set; }

    [CliOption("--from-literal")]
    public virtual string[]? FromLiteral { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliFlag("--save-config")]
    public virtual bool? SaveConfig { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public virtual string? Template { get; set; }

    [CliOption("--type")]
    public virtual string? Type { get; set; }

    [CliFlag("--validate")]
    public virtual bool? Validate { get; set; }
}