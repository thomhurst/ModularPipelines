using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata", "dc", "config", "remove")]
public record AzArcdataDcConfigRemoveOptions(
[property: CommandSwitch("--config-file")] string ConfigFile,
[property: CommandSwitch("--json-path")] string JsonPath,
[property: CommandSwitch("--path")] string Path
) : AzOptions
{
    [CommandSwitch("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CommandSwitch("--use-k8s")]
    public string? UseK8s { get; set; }
}