using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "blob")]
public class AzStorageBlobGenerateSas
{
    public AzStorageBlobGenerateSas(
        AzStorageBlobGenerateSasStorageBlobPreview storageBlobPreview
    )
    {
        StorageBlobPreview = storageBlobPreview;
    }

    public AzStorageBlobGenerateSasStorageBlobPreview StorageBlobPreview { get; }
}