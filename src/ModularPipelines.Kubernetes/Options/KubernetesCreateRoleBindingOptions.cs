using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("create", "rolebinding")]
[ExcludeFromCodeCoverage]
public record KubernetesCreateRoleBindingOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--clusterrole")]
    public virtual string? Clusterrole { get; set; }

    [CliOption("--dry-run")]
    public virtual string? DryRun { get; set; }

    [CliOption("--field-manager")]
    public virtual string? FieldManager { get; set; }

    [CliOption("--group")]
    public virtual string[]? Group { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliOption("--role")]
    public virtual string? Role { get; set; }

    [CliFlag("--save-config")]
    public virtual bool? SaveConfig { get; set; }

    [CliOption("--serviceaccount")]
    public virtual string[]? Serviceaccount { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--template")]
    public virtual string? Template { get; set; }

    [CliFlag("--validate")]
    public virtual bool? Validate { get; set; }
}