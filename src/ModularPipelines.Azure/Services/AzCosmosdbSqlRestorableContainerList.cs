using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql", "restorable-container")]
public class AzCosmosdbSqlRestorableContainerList
{
    public AzCosmosdbSqlRestorableContainerList(
        AzCosmosdbSqlRestorableContainerListCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbSqlRestorableContainerListCosmosdbPreview CosmosdbPreview { get; }
}