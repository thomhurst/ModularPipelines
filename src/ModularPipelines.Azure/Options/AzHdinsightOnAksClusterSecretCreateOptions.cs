using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight-on-aks", "cluster", "secret", "create")]
public record AzHdinsightOnAksClusterSecretCreateOptions(
[property: CommandSwitch("--reference-name")] string ReferenceName,
[property: CommandSwitch("--secret-name")] string SecretName
) : AzOptions
{
    [CommandSwitch("--version")]
    public string? Version { get; set; }
}