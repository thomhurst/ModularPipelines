using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "table")]
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