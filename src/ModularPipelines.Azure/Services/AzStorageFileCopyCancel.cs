using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "file", "copy")]
public class AzStorageFileCopyCancel
{
    public AzStorageFileCopyCancel(
        AzStorageFileCopyCancelStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageFileCopyCancelStoragePreview StoragePreview { get; }
}