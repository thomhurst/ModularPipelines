using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb")]
public class AzCosmosdbCreate
{
    public AzCosmosdbCreate(
        AzCosmosdbCreateCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbCreateCosmosdbPreview CosmosdbPreview { get; }
}