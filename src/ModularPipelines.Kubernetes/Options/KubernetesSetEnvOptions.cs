using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("set", "env")]
[ExcludeFromCodeCoverage]
public record KubernetesSetEnvOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("--all")]
    public virtual bool? All { get; set; }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CommandEqualsSeparatorSwitch("--containers", SwitchValueSeparator = " ")]
    public string? Containers { get; set; }

    [CommandEqualsSeparatorSwitch("--dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandEqualsSeparatorSwitch("--env", SwitchValueSeparator = " ")]
    public string[]? Env { get; set; }

    [CommandEqualsSeparatorSwitch("--field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandEqualsSeparatorSwitch("--filename", SwitchValueSeparator = " ")]
    public string[]? Filename { get; set; }

    [CommandEqualsSeparatorSwitch("--from", SwitchValueSeparator = " ")]
    public string? From { get; set; }

    [CommandEqualsSeparatorSwitch("--keys", SwitchValueSeparator = " ")]
    public string[]? Keys { get; set; }

    [CommandEqualsSeparatorSwitch("--kustomize", SwitchValueSeparator = " ")]
    public string? Kustomize { get; set; }

    [BooleanCommandSwitch("--list")]
    public virtual bool? List { get; set; }

    [BooleanCommandSwitch("--local")]
    public virtual bool? Local { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--overwrite")]
    public virtual bool? Overwrite { get; set; }

    [CommandEqualsSeparatorSwitch("--prefix", SwitchValueSeparator = " ")]
    public string? Prefix { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public virtual bool? Recursive { get; set; }

    [BooleanCommandSwitch("--resolve")]
    public virtual bool? Resolve { get; set; }

    [CommandEqualsSeparatorSwitch("--selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CommandEqualsSeparatorSwitch("--template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }
}