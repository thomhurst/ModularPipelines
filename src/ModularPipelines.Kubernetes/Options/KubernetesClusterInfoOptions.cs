using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("cluster-info")]
[ExcludeFromCodeCoverage]
public record KubernetesClusterInfoOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--all-namespaces")]
    public virtual bool? AllNamespaces { get; set; }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CommandEqualsSeparatorSwitch("--namespaces", SwitchValueSeparator = " ")]
    public string[]? Namespaces { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandEqualsSeparatorSwitch("--output-directory", SwitchValueSeparator = " ")]
    public string? OutputDirectory { get; set; }

    [CommandEqualsSeparatorSwitch("--pod-running-timeout", SwitchValueSeparator = " ")]
    public string? PodRunningTimeout { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CommandEqualsSeparatorSwitch("--template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }
}