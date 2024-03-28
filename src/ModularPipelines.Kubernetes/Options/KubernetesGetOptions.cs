using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesGetOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--all-namespaces")]
    public bool? AllNamespaces { get; set; }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandSwitch("--chunk-size")]
    public string? ChunkSize { get; set; }

    [CommandSwitch("--field-selector")]
    public string? FieldSelector { get; set; }

    [CommandSwitch("--filename")]
    public IEnumerable<string>? Filename { get; set; }

    [BooleanCommandSwitch("--ignore-not-found")]
    public bool? IgnoreNotFound { get; set; }

    [CommandSwitch("--kustomize")]
    public string? Kustomize { get; set; }

    [CommandSwitch("--label-columns")]
    public IEnumerable<string>? LabelColumns { get; set; }

    [PositionalArgument(PlaceholderName = "<NameLLabel>")]
    public string? NameLLabel { get; set; }

    [BooleanCommandSwitch("--no-headers")]
    public bool? NoHeaders { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--output-watch-events")]
    public bool? OutputWatchEvents { get; set; }

    [CommandSwitch("--raw")]
    public string? Raw { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [CommandSwitch("--selector")]
    public string? Selector { get; set; }

    [BooleanCommandSwitch("--server-print")]
    public bool? ServerPrint { get; set; }

    [BooleanCommandSwitch("--show-kind")]
    public bool? ShowKind { get; set; }

    [BooleanCommandSwitch("--show-labels")]
    public bool? ShowLabels { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandSwitch("--sort-by")]
    public string? SortBy { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }

    [PositionalArgument(PlaceholderName = "<Type>")]
    public string? Type { get; set; }

    [PositionalArgument(PlaceholderName = "<TypeName>")]
    public string? TypeName { get; set; }

    [BooleanCommandSwitch("--use-openapi-print-columns")]
    public bool? UseOpenapiPrintColumns { get; set; }

    [BooleanCommandSwitch("--watch")]
    public bool? Watch { get; set; }

    [BooleanCommandSwitch("--watch-only")]
    public bool? WatchOnly { get; set; }
}
