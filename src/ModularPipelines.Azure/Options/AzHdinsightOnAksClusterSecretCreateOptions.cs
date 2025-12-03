using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hdinsight-on-aks", "cluster", "secret", "create")]
public record AzHdinsightOnAksClusterSecretCreateOptions(
[property: CliOption("--reference-name")] string ReferenceName,
[property: CliOption("--secret-name")] string SecretName
) : AzOptions
{
    [CliOption("--version")]
    public string? Version { get; set; }
}