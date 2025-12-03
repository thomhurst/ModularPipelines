using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arcdata", "dc", "config", "show")]
public record AzArcdataDcConfigShowOptions : AzOptions
{
    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}