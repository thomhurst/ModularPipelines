using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arcdata", "dc", "delete")]
public record AzArcdataDcDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }

    [CliOption("--yes")]
    public bool? Yes { get; set; } = true;
}