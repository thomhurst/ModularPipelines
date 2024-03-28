using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesCreateServiceLoadbalancerOptions : KubernetesOptions
{
    public KubernetesCreateServiceLoadbalancerOptions()
    {
        CommandParts = ["create", "service", "loadbalancer"];
    }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandSwitch("--dry-run")]
    public string? DryRun { get; set; }

    [CommandSwitch("--field-manager")]
    public string? FieldManager { get; set; }

    [PositionalArgument(PlaceholderName = "<NAME>")]
    public string? NAME { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--save-config")]
    public bool? SaveConfig { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandSwitch("--tcp")]
    public IEnumerable<string>? Tcp { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }
}
