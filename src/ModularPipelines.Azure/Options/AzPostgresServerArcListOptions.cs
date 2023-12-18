using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("postgres", "server-arc", "list")]
public record AzPostgresServerArcListOptions : AzOptions
{
    [CommandSwitch("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [BooleanCommandSwitch("--use-k8s")]
    public bool? UseK8s { get; set; }
}