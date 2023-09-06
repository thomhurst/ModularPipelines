using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("create", "cronjob")]
[ExcludeFromCodeCoverage]
public record KubernetesCreateCronJobOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandEqualsSeparatorSwitch("--dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandEqualsSeparatorSwitch("--field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandEqualsSeparatorSwitch("--image", SwitchValueSeparator = " ")]
    public string? Image { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandEqualsSeparatorSwitch("--restart", SwitchValueSeparator = " ")]
    public string? Restart { get; set; }

    [BooleanCommandSwitch("--save-config")]
    public bool? SaveConfig { get; set; }

    [CommandEqualsSeparatorSwitch("--schedule", SwitchValueSeparator = " ")]
    public string? Schedule { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandEqualsSeparatorSwitch("--template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }
}
