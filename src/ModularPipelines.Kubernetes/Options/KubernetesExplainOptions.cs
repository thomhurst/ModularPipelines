using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("explain")]
public record KubernetesExplainOptions([property: PositionalArgument] string Resource) : KubernetesOptions
{
    [CommandEqualsSeparatorSwitch("--api-version", SwitchValueSeparator = " ")]
    public string? ApiVersion { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }
}
