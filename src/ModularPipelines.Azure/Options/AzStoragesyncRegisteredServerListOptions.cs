using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagesync", "registered-server", "list")]
public record AzStoragesyncRegisteredServerListOptions(
[property: CommandSwitch("--storage-sync-service")] string StorageSyncService
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}