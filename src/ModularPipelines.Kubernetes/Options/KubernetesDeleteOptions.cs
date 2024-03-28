using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesDeleteOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [BooleanCommandSwitch("--all-namespaces")]
    public bool? AllNamespaces { get; set; }

    [CommandSwitch("--cascade")]
    public string? Cascade { get; set; }

    [CommandSwitch("--dry-run")]
    public string? DryRun { get; set; }

    [CommandSwitch("--field-selector")]
    public string? FieldSelector { get; set; }

    [CommandSwitch("--filename")]
    public IEnumerable<string>? Filename { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--grace-period")]
    public int? GracePeriod { get; set; }

    [BooleanCommandSwitch("--ignore-not-found")]
    public bool? IgnoreNotFound { get; set; }

    [CommandSwitch("--kustomize")]
    public string? Kustomize { get; set; }

    [BooleanCommandSwitch("--now")]
    public bool? Now { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [CommandSwitch("--raw")]
    public string? Raw { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [CommandSwitch("--selector")]
    public string? Selector { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [PositionalArgument(PlaceholderName = "<Type>")]
    public string? Type { get; set; }

    [BooleanCommandSwitch("--wait")]
    public bool? Wait { get; set; }
}
