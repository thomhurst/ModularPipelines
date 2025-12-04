using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks", "kollect")]
public record AzAksKollectOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--container-logs")]
    public string? ContainerLogs { get; set; }

    [CliOption("--kube-objects")]
    public string? KubeObjects { get; set; }

    [CliOption("--node-logs")]
    public string? NodeLogs { get; set; }

    [CliOption("--node-logs-windows")]
    public string? NodeLogsWindows { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }
}