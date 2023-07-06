using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("explain")]
public record KubernetesExplainOptions(string Resource) : KubernetesOptions
{
    [CommandLongSwitch("api-version", SwitchValueSeparator = " ")]
    public string? ApiVersion { get; set; }

    [BooleanCommandSwitch("recursive")]
    public bool? Recursive { get; set; }

}
