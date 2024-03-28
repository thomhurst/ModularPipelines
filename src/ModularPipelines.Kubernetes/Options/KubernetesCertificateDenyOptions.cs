using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesCertificateDenyOptions : KubernetesOptions
{
    public KubernetesCertificateDenyOptions(
        IEnumerable<string> filename
)
    {
        CommandParts = ["certificate", "deny"];
        Filename = filename;
    }

    public KubernetesCertificateDenyOptions(
        string name
)
    {
        CommandParts = ["certificate", "deny", "<Name>"];
        Name = name;
    }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandSwitch("--filename")]
    public IEnumerable<string>? Filename { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--kustomize")]
    public string? Kustomize { get; set; }

    [PositionalArgument(PlaceholderName = "<Name>")]
    public string? Name { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }
}
