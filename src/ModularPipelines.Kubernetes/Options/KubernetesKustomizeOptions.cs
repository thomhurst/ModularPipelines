using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("kustomize")]
[ExcludeFromCodeCoverage]
public record KubernetesKustomizeOptions([property: PositionalArgument] string Dir) : KubernetesOptions
{
    [BooleanCommandSwitch("--as-current-user")]
    public virtual bool? AsCurrentUser { get; set; }

    [BooleanCommandSwitch("--enable-alpha-plugins")]
    public virtual bool? EnableAlphaPlugins { get; set; }

    [BooleanCommandSwitch("--enable-helm")]
    public virtual bool? EnableHelm { get; set; }

    [BooleanCommandSwitch("--enable-managedby-label")]
    public virtual bool? EnableManagedbyLabel { get; set; }

    [CommandEqualsSeparatorSwitch("--env", SwitchValueSeparator = " ")]
    public string[]? Env { get; set; }

    [CommandEqualsSeparatorSwitch("--helm-command", SwitchValueSeparator = " ")]
    public string? HelmCommand { get; set; }

    [CommandEqualsSeparatorSwitch("--load-restrictor", SwitchValueSeparator = " ")]
    public string? LoadRestrictor { get; set; }

    [CommandEqualsSeparatorSwitch("--mount", SwitchValueSeparator = " ")]
    public string[]? Mount { get; set; }

    [BooleanCommandSwitch("--network")]
    public virtual bool? Network { get; set; }

    [CommandEqualsSeparatorSwitch("--network-name", SwitchValueSeparator = " ")]
    public string? NetworkName { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandEqualsSeparatorSwitch("--reorder", SwitchValueSeparator = " ")]
    public string? Reorder { get; set; }
}