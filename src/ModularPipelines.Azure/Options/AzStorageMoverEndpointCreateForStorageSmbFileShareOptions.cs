using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage-mover", "endpoint", "create-for-storage-smb-file-share")]
public record AzStorageMoverEndpointCreateForStorageSmbFileShareOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName,
[property: CommandSwitch("--file-share-name")] string FileShareName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--storage-account-id")] int StorageAccountId,
[property: CommandSwitch("--storage-mover-name")] string StorageMoverName
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }
}

