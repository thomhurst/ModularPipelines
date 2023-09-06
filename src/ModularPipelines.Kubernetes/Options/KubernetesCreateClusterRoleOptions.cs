using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("create", "clusterrole")]
[ExcludeFromCodeCoverage]
public record KubernetesCreateClusterRoleOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [CommandEqualsSeparatorSwitch("--aggregation-rule", SwitchValueSeparator = " ")]
    public string? AggregationRule { get; set; }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandEqualsSeparatorSwitch("--dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandEqualsSeparatorSwitch("--field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandEqualsSeparatorSwitch("--non-resource-url", SwitchValueSeparator = " ")]
    public string[]? NonResourceUrl { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandEqualsSeparatorSwitch("--resource", SwitchValueSeparator = " ")]
    public string[]? Resource { get; set; }

    [CommandEqualsSeparatorSwitch("--resource-name", SwitchValueSeparator = " ")]
    public string[]? ResourceName { get; set; }

    [BooleanCommandSwitch("--save-config")]
    public bool? SaveConfig { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandEqualsSeparatorSwitch("--template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }

    [CommandEqualsSeparatorSwitch("--verb", SwitchValueSeparator = " ")]
    public string[]? Verb { get; set; }
}
