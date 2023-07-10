using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("set", "env")]
public record KubernetesSetEnvOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandLongSwitch("containers", SwitchValueSeparator = " ")]
    public string? Containers { get; set; }

    [CommandLongSwitch("dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandLongSwitch("env", SwitchValueSeparator = " ")]
    public string[]? Env { get; set; }

    [CommandLongSwitch("field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandLongSwitch("filename", SwitchValueSeparator = " ")]
    public string[]? Filename { get; set; }

    [CommandLongSwitch("from", SwitchValueSeparator = " ")]
    public string? From { get; set; }

    [CommandLongSwitch("keys", SwitchValueSeparator = " ")]
    public string[]? Keys { get; set; }

    [CommandLongSwitch("kustomize", SwitchValueSeparator = " ")]
    public string? Kustomize { get; set; }

    [BooleanCommandSwitch("list")]
    public bool? List { get; set; }

    [BooleanCommandSwitch("local")]
    public bool? Local { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("overwrite")]
    public bool? Overwrite { get; set; }

    [CommandLongSwitch("prefix", SwitchValueSeparator = " ")]
    public string? Prefix { get; set; }

    [BooleanCommandSwitch("recursive")]
    public bool? Recursive { get; set; }

    [BooleanCommandSwitch("resolve")]
    public bool? Resolve { get; set; }

    [CommandLongSwitch("selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [BooleanCommandSwitch("show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandLongSwitch("template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

}
