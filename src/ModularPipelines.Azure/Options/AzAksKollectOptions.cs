using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "kollect")]
public record AzAksKollectOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--container-logs")]
    public string? ContainerLogs { get; set; }

    [CommandSwitch("--kube-objects")]
    public string? KubeObjects { get; set; }

    [CommandSwitch("--node-logs")]
    public string? NodeLogs { get; set; }

    [CommandSwitch("--node-logs-windows")]
    public string? NodeLogsWindows { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }
}