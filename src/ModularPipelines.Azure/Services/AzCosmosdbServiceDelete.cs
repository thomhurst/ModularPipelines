using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "service")]
public class AzCosmosdbServiceDelete
{
    public AzCosmosdbServiceDelete(
        AzCosmosdbServiceDeleteCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbServiceDeleteCosmosdbPreview CosmosdbPreview { get; }
}