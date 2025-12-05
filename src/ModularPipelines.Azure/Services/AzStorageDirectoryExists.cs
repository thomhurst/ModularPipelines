using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "directory")]
public class AzStorageDirectoryExists
{
    public AzStorageDirectoryExists(
        AzStorageDirectoryExistsStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageDirectoryExistsStoragePreview StoragePreview { get; }
}