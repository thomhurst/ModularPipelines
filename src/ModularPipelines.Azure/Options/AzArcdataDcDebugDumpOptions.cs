using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

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

    [BooleanCommandSwitch("--use-k8s")]
    public bool? UseK8s { get; set; }
}