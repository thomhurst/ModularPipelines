using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("config", "unset")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigUnsetOptions([property: PositionalArgument] string Property_name) : KubernetesOptions
{
    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [BooleanCommandSwitch("--flatten")]
    public virtual bool? Flatten { get; set; }

    [BooleanCommandSwitch("--merge")]
    public virtual bool? Merge { get; set; }

    [BooleanCommandSwitch("--minify")]
    public virtual bool? Minify { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--raw")]
    public virtual bool? Raw { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CommandEqualsSeparatorSwitch("--template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }
}