using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesCreateGenericOptions : KubernetesOptions
{
    public KubernetesCreateGenericOptions()
    {
        CommandParts = ["create", "generic"];
    }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [BooleanCommandSwitch("--append-hash")]
    public bool? AppendHash { get; set; }

    [CommandSwitch("--dry-run")]
    public string? DryRun { get; set; }

    [CommandSwitch("--field-manager")]
    public string? FieldManager { get; set; }

    [CommandSwitch("--from-env-file")]
    public string? FromEnvFile { get; set; }

    [CommandSwitch("--from-file")]
    public IEnumerable<string>? FromFile { get; set; }

    [CommandSwitch("--from-literal")]
    public IEnumerable<string>? FromLiteral { get; set; }

    [PositionalArgument(PlaceholderName = "<NAME>")]
    public string? NAME { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--save-config")]
    public bool? SaveConfig { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }
}
