using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

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