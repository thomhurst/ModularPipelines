using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata", "dc", "export")]
public record AzArcdataDcExportOptions(
[property: CommandSwitch("--k8s-namespace")] string K8sNamespace,
[property: CommandSwitch("--path")] string Path,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--use-k8s")]
    public string? UseK8s { get; set; }
}

