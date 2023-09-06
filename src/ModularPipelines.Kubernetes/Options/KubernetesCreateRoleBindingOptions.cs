using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("create", "rolebinding")]
[ExcludeFromCodeCoverage]
public record KubernetesCreateRoleBindingOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandEqualsSeparatorSwitch("--clusterrole", SwitchValueSeparator = " ")]
    public string? Clusterrole { get; set; }

    [CommandEqualsSeparatorSwitch("--dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandEqualsSeparatorSwitch("--field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandEqualsSeparatorSwitch("--group", SwitchValueSeparator = " ")]
    public string[]? Group { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandEqualsSeparatorSwitch("--role", SwitchValueSeparator = " ")]
    public string? Role { get; set; }

    [BooleanCommandSwitch("--save-config")]
    public bool? SaveConfig { get; set; }

    [CommandEqualsSeparatorSwitch("--serviceaccount", SwitchValueSeparator = " ")]
    public string[]? Serviceaccount { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandEqualsSeparatorSwitch("--template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }
}
