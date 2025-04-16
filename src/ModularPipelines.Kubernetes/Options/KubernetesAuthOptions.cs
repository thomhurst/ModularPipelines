using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("auth")]
[ExcludeFromCodeCoverage]
public record KubernetesAuthOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--all-namespaces")]
    public virtual bool? AllNamespaces { get; set; }

    [BooleanCommandSwitch("--list")]
    public virtual bool? List { get; set; }

    [BooleanCommandSwitch("--no-headers")]
    public virtual bool? NoHeaders { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public virtual bool? Quiet { get; set; }

    [CommandEqualsSeparatorSwitch("--subresource", SwitchValueSeparator = " ")]
    public string? Subresource { get; set; }
}