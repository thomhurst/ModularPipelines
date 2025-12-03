using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "directory")]
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