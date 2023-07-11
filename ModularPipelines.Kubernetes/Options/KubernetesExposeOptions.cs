using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("expose")]
public record KubernetesExposeOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandEqualsSeparatorSwitch("--cluster-ip", SwitchValueSeparator = " ")]
    public string? ClusterIp { get; set; }

    [CommandEqualsSeparatorSwitch("--container-port", SwitchValueSeparator = " ")]
    public string? ContainerPort { get; set; }

    [CommandEqualsSeparatorSwitch("--dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandEqualsSeparatorSwitch("--external-ip", SwitchValueSeparator = " ")]
    public string? ExternalIp { get; set; }

    [CommandEqualsSeparatorSwitch("--field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandEqualsSeparatorSwitch("--filename", SwitchValueSeparator = " ")]
    public string[]? Filename { get; set; }

    [CommandEqualsSeparatorSwitch("--generator", SwitchValueSeparator = " ")]
    public string? Generator { get; set; }

    [CommandEqualsSeparatorSwitch("--kustomize", SwitchValueSeparator = " ")]
    public string? Kustomize { get; set; }

    [CommandEqualsSeparatorSwitch("--labels", SwitchValueSeparator = " ")]
    public string? Labels { get; set; }

    [CommandEqualsSeparatorSwitch("--load-balancer-ip", SwitchValueSeparator = " ")]
    public string? LoadBalancerIp { get; set; }

    [CommandEqualsSeparatorSwitch("--name", SwitchValueSeparator = " ")]
    public string? Name { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandEqualsSeparatorSwitch("--overrides", SwitchValueSeparator = " ")]
    public string? Overrides { get; set; }

    [CommandEqualsSeparatorSwitch("--port", SwitchValueSeparator = " ")]
    public string? Port { get; set; }

    [CommandEqualsSeparatorSwitch("--protocol", SwitchValueSeparator = " ")]
    public string? Protocol { get; set; }

    [BooleanCommandSwitch("--record")]
    public bool? Record { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [BooleanCommandSwitch("--save-config")]
    public bool? SaveConfig { get; set; }

    [CommandEqualsSeparatorSwitch("--selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [CommandEqualsSeparatorSwitch("--session-affinity", SwitchValueSeparator = " ")]
    public string? SessionAffinity { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandEqualsSeparatorSwitch("--target-port", SwitchValueSeparator = " ")]
    public string? TargetPort { get; set; }

    [CommandEqualsSeparatorSwitch("--template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

    [CommandEqualsSeparatorSwitch("--type", SwitchValueSeparator = " ")]
    public string? Type { get; set; }
}
