using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesApplySetLastAppliedOptions : KubernetesOptions
{
    public KubernetesApplySetLastAppliedOptions(
        IEnumerable<string> filename
)
    {
        CommandParts = ["apply", "set-last-applied"];
        Filename = filename;
    }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [BooleanCommandSwitch("--create-annotation")]
    public bool? CreateAnnotation { get; set; }

    [CommandSwitch("--dry-run")]
    public string? DryRun { get; set; }

    [CommandSwitch("--filename")]
    public IEnumerable<string>? Filename { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }
}
