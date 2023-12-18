using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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