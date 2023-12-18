using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata", "dc", "debug", "dump")]
public record AzArcdataDcDebugDumpOptions(
[property: CommandSwitch("--k8s-namespace")] string K8sNamespace
) : AzOptions
{
    [CommandSwitch("--container")]
    public string? Container { get; set; }

    [CommandSwitch("--target-folder")]
    public string? TargetFolder { get; set; }

    [CommandSwitch("--use-k8s")]
    public string? UseK8s { get; set; }
}

