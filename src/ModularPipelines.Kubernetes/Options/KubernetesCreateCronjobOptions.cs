using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesCreateCronjobOptions : KubernetesOptions
{
    public KubernetesCreateCronjobOptions(
        string image,
        string schedule
)
    {
        CommandParts = ["create", "cronjob"];
        Image = image;
        Schedule = schedule;
    }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [PositionalArgument(PlaceholderName = "<Command>")]
    public string? Command { get; set; }

    [CommandSwitch("--dry-run")]
    public string? DryRun { get; set; }

    [CommandSwitch("--field-manager")]
    public string? FieldManager { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [PositionalArgument(PlaceholderName = "<NAME>")]
    public string? NAME { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [CommandSwitch("--restart")]
    public string? Restart { get; set; }

    [BooleanCommandSwitch("--save-config")]
    public bool? SaveConfig { get; set; }

    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }
}
