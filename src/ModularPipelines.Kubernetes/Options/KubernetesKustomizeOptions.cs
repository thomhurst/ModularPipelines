using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesKustomizeOptions : KubernetesOptions
{
    public KubernetesKustomizeOptions()
    {
        CommandParts = ["kustomize"];
    }

    [BooleanCommandSwitch("--as-current-user")]
    public bool? AsCurrentUser { get; set; }

    [PositionalArgument(PlaceholderName = "<DIR>")]
    public string? DIR { get; set; }

    [BooleanCommandSwitch("--enable-alpha-plugins")]
    public bool? EnableAlphaPlugins { get; set; }

    [BooleanCommandSwitch("--enable-helm")]
    public bool? EnableHelm { get; set; }

    [BooleanCommandSwitch("--enable-managedby-label")]
    public bool? EnableManagedbyLabel { get; set; }

    [CommandSwitch("--env")]
    public IEnumerable<string>? Env { get; set; }

    [CommandSwitch("--helm-command")]
    public string? HelmCommand { get; set; }

    [CommandSwitch("--load-restrictor")]
    public string? LoadRestrictor { get; set; }

    [CommandSwitch("--mount")]
    public IEnumerable<string>? Mount { get; set; }

    [BooleanCommandSwitch("--network")]
    public bool? Network { get; set; }

    [CommandSwitch("--network-name")]
    public string? NetworkName { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [CommandSwitch("--reorder")]
    public string? Reorder { get; set; }
}
