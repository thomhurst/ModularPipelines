using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesCreateClusterroleOptions : KubernetesOptions
{
    public KubernetesCreateClusterroleOptions(
        IEnumerable<string> verb,
        IEnumerable<string> resource
)
    {
        CommandParts = ["create", "clusterrole"];
        Verb = verb;
        Resource = resource;
    }

    [CommandSwitch("--aggregation-rule")]
    public string? AggregationRule { get; set; }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandSwitch("--dry-run")]
    public string? DryRun { get; set; }

    [CommandSwitch("--field-manager")]
    public string? FieldManager { get; set; }

    [PositionalArgument(PlaceholderName = "<NAME>")]
    public string? NAME { get; set; }

    [CommandSwitch("--non-resource-url")]
    public IEnumerable<string>? NonResourceUrl { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [CommandSwitch("--resource")]
    public IEnumerable<string>? Resource { get; set; }

    [CommandSwitch("--resource-name")]
    public IEnumerable<string>? ResourceName { get; set; }

    [BooleanCommandSwitch("--save-config")]
    public bool? SaveConfig { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }

    [CommandSwitch("--verb")]
    public IEnumerable<string>? Verb { get; set; }
}
