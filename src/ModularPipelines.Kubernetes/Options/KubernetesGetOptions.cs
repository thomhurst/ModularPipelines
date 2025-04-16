using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("get")]
[ExcludeFromCodeCoverage]
public record KubernetesGetOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("--all-namespaces")]
    public virtual bool? AllNamespaces { get; set; }

    [BooleanCommandSwitch("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CommandEqualsSeparatorSwitch("--chunk-size", SwitchValueSeparator = " ")]
    public int? ChunkSize { get; set; }

    [CommandEqualsSeparatorSwitch("--field-selector", SwitchValueSeparator = " ")]
    public string? FieldSelector { get; set; }

    [CommandEqualsSeparatorSwitch("--filename", SwitchValueSeparator = " ")]
    public string[]? Filename { get; set; }

    [BooleanCommandSwitch("--ignore-not-found")]
    public virtual bool? IgnoreNotFound { get; set; }

    [CommandEqualsSeparatorSwitch("--kustomize", SwitchValueSeparator = " ")]
    public string? Kustomize { get; set; }

    [CommandEqualsSeparatorSwitch("--label-columns", SwitchValueSeparator = " ")]
    public string[]? LabelColumns { get; set; }

    [BooleanCommandSwitch("--no-headers")]
    public virtual bool? NoHeaders { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--output-watch-events")]
    public virtual bool? OutputWatchEvents { get; set; }

    [CommandEqualsSeparatorSwitch("--raw", SwitchValueSeparator = " ")]
    public string? Raw { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CommandEqualsSeparatorSwitch("--selector", SwitchValueSeparator = " ")]
    public string? Selector { get; set; }

    [BooleanCommandSwitch("--server-print")]
    public virtual bool? ServerPrint { get; set; }

    [BooleanCommandSwitch("--show-kind")]
    public virtual bool? ShowKind { get; set; }

    [BooleanCommandSwitch("--show-labels")]
    public virtual bool? ShowLabels { get; set; }

    [BooleanCommandSwitch("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CommandEqualsSeparatorSwitch("--sort-by", SwitchValueSeparator = " ")]
    public string? SortBy { get; set; }

    [CommandEqualsSeparatorSwitch("--template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("--use-openapi-print-columns")]
    public virtual bool? UseOpenapiPrintColumns { get; set; }

    [BooleanCommandSwitch("--watch")]
    public virtual bool? Watch { get; set; }

    [BooleanCommandSwitch("--watch-only")]
    public virtual bool? WatchOnly { get; set; }
}