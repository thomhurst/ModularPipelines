using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql", "container")]
public class AzCosmosdbSqlContainerUpdate
{
    public AzCosmosdbSqlContainerUpdate(
        AzCosmosdbSqlContainerUpdateCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbSqlContainerUpdateCosmosdbPreview CosmosdbPreview { get; }
}