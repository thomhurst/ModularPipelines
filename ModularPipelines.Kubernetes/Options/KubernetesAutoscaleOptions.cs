using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("autoscale")]
public record KubernetesAutoscaleOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandEqualsSeparatorSwitch("--cpu-percent", SwitchValueSeparator = " ")]
    public int? CpuPercent { get; set; }

    [CommandEqualsSeparatorSwitch("--dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandEqualsSeparatorSwitch("--field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandEqualsSeparatorSwitch("--filename", SwitchValueSeparator = " ")]
    public string[]? Filename { get; set; }

    [CommandEqualsSeparatorSwitch("--kustomize", SwitchValueSeparator = " ")]
    public string? Kustomize { get; set; }

    [CommandEqualsSeparatorSwitch("--max", SwitchValueSeparator = " ")]
    public int? Max { get; set; }

    [CommandEqualsSeparatorSwitch("--min", SwitchValueSeparator = " ")]
    public int? Min { get; set; }

    [CommandEqualsSeparatorSwitch("--name", SwitchValueSeparator = " ")]
    public string? Name { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--record")]
    public bool? Record { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [BooleanCommandSwitch("--save-config")]
    public bool? SaveConfig { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandEqualsSeparatorSwitch("--template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

}
