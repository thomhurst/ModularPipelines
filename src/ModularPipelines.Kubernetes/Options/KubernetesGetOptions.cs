using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("get")]
[ExcludeFromCodeCoverage]
public record KubernetesGetOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--all-namespaces")]
    public virtual bool? AllNamespaces { get; set; }

    [CliFlag("--allow-missing-template-keys")]
    public virtual bool? AllowMissingTemplateKeys { get; set; }

    [CliOption("--chunk-size")]
    public int? ChunkSize { get; set; }

    [CliOption("--field-selector")]
    public string? FieldSelector { get; set; }

    [CliOption("--filename")]
    public string[]? Filename { get; set; }

    [CliFlag("--ignore-not-found")]
    public virtual bool? IgnoreNotFound { get; set; }

    [CliOption("--kustomize")]
    public string? Kustomize { get; set; }

    [CliOption("--label-columns")]
    public string[]? LabelColumns { get; set; }

    [CliFlag("--no-headers")]
    public virtual bool? NoHeaders { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliFlag("--output-watch-events")]
    public virtual bool? OutputWatchEvents { get; set; }

    [CliOption("--raw")]
    public string? Raw { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliOption("--selector")]
    public string? Selector { get; set; }

    [CliFlag("--server-print")]
    public virtual bool? ServerPrint { get; set; }

    [CliFlag("--show-kind")]
    public virtual bool? ShowKind { get; set; }

    [CliFlag("--show-labels")]
    public virtual bool? ShowLabels { get; set; }

    [CliFlag("--show-managed-fields")]
    public virtual bool? ShowManagedFields { get; set; }

    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--template")]
    public string? Template { get; set; }

    [CliFlag("--use-openapi-print-columns")]
    public virtual bool? UseOpenapiPrintColumns { get; set; }

    [CliFlag("--watch")]
    public virtual bool? Watch { get; set; }

    [CliFlag("--watch-only")]
    public virtual bool? WatchOnly { get; set; }
}