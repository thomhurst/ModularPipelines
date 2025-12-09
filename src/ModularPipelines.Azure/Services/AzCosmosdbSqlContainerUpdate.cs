using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "sql", "container")]
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