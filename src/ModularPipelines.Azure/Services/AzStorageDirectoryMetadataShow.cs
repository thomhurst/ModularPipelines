using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "directory", "metadata")]
public class AzStorageDirectoryMetadataShow
{
    public AzStorageDirectoryMetadataShow(
        AzStorageDirectoryMetadataShowStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageDirectoryMetadataShowStoragePreview StoragePreview { get; }
}

