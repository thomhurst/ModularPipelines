using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "gremlin")]
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