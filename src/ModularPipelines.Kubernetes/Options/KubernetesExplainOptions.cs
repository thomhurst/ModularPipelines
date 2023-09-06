using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("explain")]
[ExcludeFromCodeCoverage]
public record KubernetesExplainOptions([property: PositionalArgument] string Resource) : KubernetesOptions
{
    [CommandEqualsSeparatorSwitch("--api-version", SwitchValueSeparator = " ")]
    public string? ApiVersion { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }
}
