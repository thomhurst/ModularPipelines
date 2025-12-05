using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "blob")]
public class AzStorageBlobShow
{
    public AzStorageBlobShow(
        AzStorageBlobShowStorageBlobPreview storageBlobPreview
    )
    {
        StorageBlobPreview = storageBlobPreview;
    }

    public AzStorageBlobShowStorageBlobPreview StorageBlobPreview { get; }
}