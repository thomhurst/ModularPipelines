using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage-mover", "endpoint", "create-for-storage-container")]
public record AzStorageMoverEndpointCreateForStorageContainerOptions(
[property: CommandSwitch("--container-name")] string ContainerName,
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-account-id")] int StorageAccountId,
[property: CommandSwitch("--storage-mover-name")] string StorageMoverName
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }
}