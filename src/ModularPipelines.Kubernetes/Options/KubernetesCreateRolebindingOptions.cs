using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesCreateRolebindingOptions : KubernetesOptions
{
    public KubernetesCreateRolebindingOptions(
        string clusterrole
)
    {
        CommandParts = ["create", "rolebinding"];
        Clusterrole = clusterrole;
    }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandSwitch("--clusterrole")]
    public string? Clusterrole { get; set; }

    [CommandSwitch("--dry-run")]
    public string? DryRun { get; set; }

    [CommandSwitch("--field-manager")]
    public string? FieldManager { get; set; }

    [CommandSwitch("--group")]
    public IEnumerable<string>? Group { get; set; }

    [PositionalArgument(PlaceholderName = "<NAME>")]
    public string? NAME { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [BooleanCommandSwitch("--save-config")]
    public bool? SaveConfig { get; set; }

    [CommandSwitch("--serviceaccount")]
    public IEnumerable<string>? Serviceaccount { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }
}
