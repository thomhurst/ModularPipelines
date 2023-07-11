using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("cluster-info", "dump")]
public record KubernetesClusterInfoDumpOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--all-namespaces")]
    public bool? AllNamespaces { get; set; }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandEqualsSeparatorSwitch("--namespaces", SwitchValueSeparator = " ")]
    public string[]? Namespaces { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandEqualsSeparatorSwitch("--output-directory", SwitchValueSeparator = " ")]
    public string? OutputDirectory { get; set; }

    [CommandEqualsSeparatorSwitch("--pod-running-timeout", SwitchValueSeparator = " ")]
    public string? PodRunningTimeout { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandEqualsSeparatorSwitch("--template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

}
