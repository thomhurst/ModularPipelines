using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesApplyEditLastAppliedOptions : KubernetesOptions
{
    public KubernetesApplyEditLastAppliedOptions(
        string resourceName
)
    {
        CommandParts = ["apply", "edit-last-applied", "<ResourceName>"];
        ResourceName = resourceName;
    }

    public KubernetesApplyEditLastAppliedOptions(
        IEnumerable<string> filename
)
    {
        CommandParts = ["apply", "edit-last-applied"];
        Filename = filename;
    }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandSwitch("--field-manager")]
    public string? FieldManager { get; set; }

    [CommandSwitch("--filename")]
    public IEnumerable<string>? Filename { get; set; }

    [CommandSwitch("--kustomize")]
    public string? Kustomize { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--record")]
    public bool? Record { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [PositionalArgument(PlaceholderName = "<ResourceName>")]
    public string? ResourceName { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("--windows-line-endings")]
    public bool? WindowsLineEndings { get; set; }
}
