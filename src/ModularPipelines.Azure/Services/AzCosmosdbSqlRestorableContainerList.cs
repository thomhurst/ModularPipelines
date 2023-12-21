using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

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