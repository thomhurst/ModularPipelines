using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "sql", "container")]
public class AzCosmosdbSqlContainerCreate
{
    public AzCosmosdbSqlContainerCreate(
        AzCosmosdbSqlContainerCreateCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbSqlContainerCreateCosmosdbPreview CosmosdbPreview { get; }
}