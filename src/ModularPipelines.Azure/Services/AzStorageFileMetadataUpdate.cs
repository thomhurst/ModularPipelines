using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "file", "metadata")]
public class AzStorageFileMetadataUpdate
{
    public AzStorageFileMetadataUpdate(
        AzStorageFileMetadataUpdateStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageFileMetadataUpdateStoragePreview StoragePreview { get; }
}