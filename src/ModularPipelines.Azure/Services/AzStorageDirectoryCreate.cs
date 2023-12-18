using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "directory")]
public class AzStorageDirectoryCreate
{
    public AzStorageDirectoryCreate(
        AzStorageDirectoryCreateStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageDirectoryCreateStoragePreview StoragePreview { get; }
}

