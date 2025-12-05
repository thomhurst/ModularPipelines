using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "file")]
public class AzStorageFileExists
{
    public AzStorageFileExists(
        AzStorageFileExistsStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageFileExistsStoragePreview StoragePreview { get; }
}