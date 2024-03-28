using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesAutoscaleOptions : KubernetesOptions
{
    public KubernetesAutoscaleOptions(
        IEnumerable<string> filename,
        int max
)
    {
        CommandParts = ["autoscale"];
        Filename = filename;
        Max = max;
    }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandSwitch("--cpu-percent")]
    public int? CpuPercent { get; set; }

    [CommandSwitch("--dry-run")]
    public string? DryRun { get; set; }

    [CommandSwitch("--field-manager")]
    public string? FieldManager { get; set; }

    [CommandSwitch("--filename")]
    public IEnumerable<string>? Filename { get; set; }

    [CommandSwitch("--kustomize")]
    public string? Kustomize { get; set; }

    [CommandSwitch("--max")]
    public int? Max { get; set; }

    [CommandSwitch("--min")]
    public int? Min { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--record")]
    public bool? Record { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [BooleanCommandSwitch("--save-config")]
    public bool? SaveConfig { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }

    [PositionalArgument(PlaceholderName = "<TypeName>")]
    public string? TypeName { get; set; }
}
