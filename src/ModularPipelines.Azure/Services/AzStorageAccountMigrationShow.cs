using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "account", "migration")]
public class AzStorageAccountMigrationShow
{
    public AzStorageAccountMigrationShow(
        AzStorageAccountMigrationShowStoragePreview storagePreview
    )
    {
        StoragePreview = storagePreview;
    }

    public AzStorageAccountMigrationShowStoragePreview StoragePreview { get; }
}