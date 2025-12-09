using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb")]
public class AzCosmosdbList
{
    public AzCosmosdbList(
        AzCosmosdbListCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbListCosmosdbPreview CosmosdbPreview { get; }
}