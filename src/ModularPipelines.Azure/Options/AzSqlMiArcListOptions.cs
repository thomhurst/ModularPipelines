using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "mi-arc", "list")]
public record AzSqlMiArcListOptions : AzOptions
{
    [CliOption("--custom-location")]
    public string? CustomLocation { get; set; }

    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}