using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("postgres", "server-arc", "list")]
public record AzPostgresServerArcListOptions(
[property: CommandSwitch("--k8s-namespace")] string K8sNamespace,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--source-server")] string SourceServer
) : AzOptions
{
    [CommandSwitch("--use-k8s")]
    public string? UseK8s { get; set; }
}