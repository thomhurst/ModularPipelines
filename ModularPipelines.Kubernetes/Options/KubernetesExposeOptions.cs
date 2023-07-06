using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("expose")]
public record KubernetesExposeOptions : KubernetesOptions
{
    [BooleanCommandSwitch("allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandLongSwitch("cluster-ip", SwitchValueSeparator = " ")]
    public string? ClusterIp { get; set; }

    [CommandLongSwitch("container-port", SwitchValueSeparator = " ")]
    public string? ContainerPort { get; set; }

    [CommandLongSwitch("dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandLongSwitch("external-ip", SwitchValueSeparator = " ")]
    public string? ExternalIp { get; set; }

    [CommandLongSwitch("field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandLongSwitch("filename", SwitchValueSeparator = " ")]
    public string[]? Filename { get; set; }

    [CommandLongSwitch("generator", SwitchValueSeparator = " ")]
    public string? Generator { get; set; }

    [CommandLongSwitch("kustomize", SwitchValueSeparator = " ")]
    public string? Kustomize { get; set; }

    [CommandLongSwitch("labels", SwitchValueSeparator = " ")]
    public string? Labels { get; set; }

    [CommandLongSwitch("load-balancer-ip", SwitchValueSeparator = " ")]
    public string? LoadBalancerIp { get; set; }

    [CommandLongSwitch("name", SwitchValueSeparator = " ")]
    public string? Name { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandLongSwitch("overrides", SwitchValueSeparator = " ")]
    public string? Overrides { get; set; }

    [CommandLongSwitch("port", SwitchValueSeparator = " ")]
    public string? Port { get; set; }

    [CommandLongSwitch("protocol", SwitchValueSeparator = " ")]
    public string? Protocol { get; set; }

    [BooleanCommandSwitch("record")]
    public bool? Record { get; set; }

    [BooleanCommandSwitch("recursive")]
    public bool? Recursive { get; set; }

    [BooleanCommandSwitch("save-config")]
    public bool? SaveConfig { get; set; }

    [CommandLongSwitch("selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [CommandLongSwitch("session-affinity", SwitchValueSeparator = " ")]
    public string? SessionAffinity { get; set; }

    [BooleanCommandSwitch("show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandLongSwitch("target-port", SwitchValueSeparator = " ")]
    public string? TargetPort { get; set; }

    [CommandLongSwitch("template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

    [CommandLongSwitch("type", SwitchValueSeparator = " ")]
    public string? Type { get; set; }

}
