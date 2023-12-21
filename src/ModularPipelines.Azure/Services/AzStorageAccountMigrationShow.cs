using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "migration")]
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