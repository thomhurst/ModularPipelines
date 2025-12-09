using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "directory")]
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