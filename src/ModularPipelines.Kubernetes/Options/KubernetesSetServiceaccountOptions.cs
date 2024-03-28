using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesSetServiceaccountOptions : KubernetesOptions
{
    public KubernetesSetServiceaccountOptions(
        IEnumerable<string> filename
)
    {
        CommandParts = ["set", "serviceaccount"];
        Filename = filename;
    }

    public KubernetesSetServiceaccountOptions(
        string typeName
)
    {
        CommandParts = ["set", "serviceaccount", "<TypeName>"];
        TypeName = typeName;
    }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandSwitch("--dry-run")]
    public string? DryRun { get; set; }

    [CommandSwitch("--field-manager")]
    public string? FieldManager { get; set; }

    [CommandSwitch("--filename")]
    public IEnumerable<string>? Filename { get; set; }

    [CommandSwitch("--kustomize")]
    public string? Kustomize { get; set; }

    [BooleanCommandSwitch("--local")]
    public bool? Local { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--record")]
    public bool? Record { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }

    [PositionalArgument(PlaceholderName = "<TypeName>")]
    public string? TypeName { get; set; }
}
