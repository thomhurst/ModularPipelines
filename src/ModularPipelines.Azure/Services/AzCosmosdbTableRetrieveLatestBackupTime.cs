using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "table")]
public class AzCosmosdbTableRetrieveLatestBackupTime
{
    public AzCosmosdbTableRetrieveLatestBackupTime(
        AzCosmosdbTableRetrieveLatestBackupTimeCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbTableRetrieveLatestBackupTimeCosmosdbPreview CosmosdbPreview { get; }
}