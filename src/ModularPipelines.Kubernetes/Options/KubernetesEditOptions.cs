using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesEditOptions : KubernetesOptions
{
    public KubernetesEditOptions(
        string resourceName
)
    {
        CommandParts = ["edit", "<ResourceName>"];
        ResourceName = resourceName;
    }

    public KubernetesEditOptions(
        IEnumerable<string> filename
)
    {
        CommandParts = ["edit"];
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

    [BooleanCommandSwitch("--output-patch")]
    public bool? OutputPatch { get; set; }

    [BooleanCommandSwitch("--record")]
    public bool? Record { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [PositionalArgument(PlaceholderName = "<ResourceName>")]
    public string? ResourceName { get; set; }

    [BooleanCommandSwitch("--save-config")]
    public bool? SaveConfig { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }

    [BooleanCommandSwitch("--windows-line-endings")]
    public bool? WindowsLineEndings { get; set; }
}
