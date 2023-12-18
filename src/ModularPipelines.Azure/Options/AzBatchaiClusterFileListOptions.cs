using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batchai", "cluster", "file", "list")]
public record AzBatchaiClusterFileListOptions(
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace")] string Workspace
) : AzOptions
{
    [CommandSwitch("--expiry")]
    public string? Expiry { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }
}