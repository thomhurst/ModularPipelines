using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "file")]
public class AzStorageFileList
{
    public AzStorageFileList(
        AzStorageFileListStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageFileListStoragePreview StoragePreview { get; }
}