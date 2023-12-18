using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagesync", "registered-server", "show")]
public record AzStoragesyncRegisteredServerShowOptions(
[property: CommandSwitch("--server-id")] string ServerId,
[property: CommandSwitch("--storage-sync-service")] string StorageSyncService
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}