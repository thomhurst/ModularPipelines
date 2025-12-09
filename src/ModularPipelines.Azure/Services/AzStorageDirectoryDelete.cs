using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "directory")]
public class AzStorageDirectoryDelete
{
    public AzStorageDirectoryDelete(
        AzStorageDirectoryDeleteStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageDirectoryDeleteStoragePreview StoragePreview { get; }
}