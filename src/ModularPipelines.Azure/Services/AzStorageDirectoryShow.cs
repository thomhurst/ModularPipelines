using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "directory")]
public class AzStorageDirectoryShow
{
    public AzStorageDirectoryShow(
        AzStorageDirectoryShowStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageDirectoryShowStoragePreview StoragePreview { get; }
}