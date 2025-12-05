using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "directory")]
public class AzStorageDirectoryList
{
    public AzStorageDirectoryList(
        AzStorageDirectoryListStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageDirectoryListStoragePreview StoragePreview { get; }
}