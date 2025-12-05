using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "blob")]
public class AzStorageBlobSetTier
{
    public AzStorageBlobSetTier(
        AzStorageBlobSetTierStorageBlobPreview storageBlobPreview
    )
    {
        StorageBlobPreview = storageBlobPreview;
    }

    public AzStorageBlobSetTierStorageBlobPreview StorageBlobPreview { get; }
}