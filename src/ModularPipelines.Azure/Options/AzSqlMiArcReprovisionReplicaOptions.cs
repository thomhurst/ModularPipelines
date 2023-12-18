using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi-arc", "reprovision-replica")]
public record AzSqlMiArcReprovisionReplicaOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--use-k8s")]
    public bool? UseK8s { get; set; }
}