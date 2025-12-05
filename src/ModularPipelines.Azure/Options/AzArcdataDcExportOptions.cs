using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcdata", "dc", "export")]
public record AzArcdataDcExportOptions(
[property: CliOption("--k8s-namespace")] string K8sNamespace,
[property: CliOption("--path")] string Path,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}