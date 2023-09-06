using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("get")]
[ExcludeFromCodeCoverage]
public record KubernetesGetOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("--all-namespaces")]
    public bool? AllNamespaces { get; set; }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public bool? AllowMissingTemplateKeys { get; set; }

    [CommandEqualsSeparatorSwitch("--chunk-size", SwitchValueSeparator = " ")]
    public int? ChunkSize { get; set; }

    [CommandEqualsSeparatorSwitch("--field-selector", SwitchValueSeparator = " ")]
    public string? FieldSelector { get; set; }

    [CommandEqualsSeparatorSwitch("--filename", SwitchValueSeparator = " ")]
    public string[]? Filename { get; set; }

    [BooleanCommandSwitch("--ignore-not-found")]
    public bool? IgnoreNotFound { get; set; }

    [CommandEqualsSeparatorSwitch("--kustomize", SwitchValueSeparator = " ")]
    public string? Kustomize { get; set; }

    [CommandEqualsSeparatorSwitch("--label-columns", SwitchValueSeparator = " ")]
    public string[]? LabelColumns { get; set; }

    [BooleanCommandSwitch("--no-headers")]
    public bool? NoHeaders { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--output-watch-events")]
    public bool? OutputWatchEvents { get; set; }

    [CommandEqualsSeparatorSwitch("--raw", SwitchValueSeparator = " ")]
    public string? Raw { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [CommandEqualsSeparatorSwitch("--selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [BooleanCommandSwitch("--server-print")]
    public bool? ServerPrint { get; set; }

    [BooleanCommandSwitch("--show-kind")]
    public bool? ShowKind { get; set; }

    [BooleanCommandSwitch("--show-labels")]
    public bool? ShowLabels { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public bool? ShowManagedFields { get; set; }

    [CommandEqualsSeparatorSwitch("--sort-by", SwitchValueSeparator = " ")]
    public string? SortBy { get; set; }

    [CommandEqualsSeparatorSwitch("--template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("--use-openapi-print-columns")]
    public bool? UseOpenapiPrintColumns { get; set; }

    [BooleanCommandSwitch("--watch")]
    public bool? Watch { get; set; }

    [BooleanCommandSwitch("--watch-only")]
    public bool? WatchOnly { get; set; }
}
