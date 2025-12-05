using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "gremlin")]
public class AzCosmosdbGremlinRetrieveLatestBackupTime
{
    public AzCosmosdbGremlinRetrieveLatestBackupTime(
        AzCosmosdbGremlinRetrieveLatestBackupTimeCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbGremlinRetrieveLatestBackupTimeCosmosdbPreview CosmosdbPreview { get; }
}