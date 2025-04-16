using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("create", "generic")]
[ExcludeFromCodeCoverage]
public record KubernetesCreateSecretGenericOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [BooleanCommandSwitch("--append-hash")]
    public virtual bool? AppendHash { get; set; }

    [CommandEqualsSeparatorSwitch("--dry-run", SwitchValueSeparator = " ")]
    public string? DryRun { get; set; }

    [CommandEqualsSeparatorSwitch("--field-manager", SwitchValueSeparator = " ")]
    public string? FieldManager { get; set; }

    [CommandEqualsSeparatorSwitch("--from-env-file", SwitchValueSeparator = " ")]
    public string? FromEnvFile { get; set; }

    [CommandEqualsSeparatorSwitch("--from-file", SwitchValueSeparator = " ")]
    public string[]? FromFile { get; set; }

    [CommandEqualsSeparatorSwitch("--from-literal", SwitchValueSeparator = " ")]
    public string[]? FromLiteral { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--save-config")]
    public virtual bool? SaveConfig { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CommandEqualsSeparatorSwitch("--template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

    [CommandEqualsSeparatorSwitch("--type", SwitchValueSeparator = " ")]
    public string? Type { get; set; }

    [BooleanCommandSwitch("--validate")]
    public virtual bool? Validate { get; set; }
}