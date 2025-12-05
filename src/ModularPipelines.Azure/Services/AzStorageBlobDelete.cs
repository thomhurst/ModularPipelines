using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "blob")]
public class AzStorageBlobDelete
{
    public AzStorageBlobDelete(
        AzStorageBlobDeleteStorageBlobPreview storageBlobPreview
    )
    {
        StorageBlobPreview = storageBlobPreview;
    }

    public AzStorageBlobDeleteStorageBlobPreview StorageBlobPreview { get; }
}