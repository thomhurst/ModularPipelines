using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "container")]
public class AzStorageContainerGenerateSas
{
    public AzStorageContainerGenerateSas(
        AzStorageContainerGenerateSasStorageBlobPreview storageBlobPreview
    )
    {
        StorageBlobPreview = storageBlobPreview;
    }

    public AzStorageContainerGenerateSasStorageBlobPreview StorageBlobPreview { get; }
}