using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "mongodb", "role", "definition")]
public class AzCosmosdbMongodbRoleDefinitionCreate
{
    public AzCosmosdbMongodbRoleDefinitionCreate(
        AzCosmosdbMongodbRoleDefinitionCreateCosmosdbPreview cosmosdbPreview
    )
    {
        CosmosdbPreview = cosmosdbPreview;
    }

    public AzCosmosdbMongodbRoleDefinitionCreateCosmosdbPreview CosmosdbPreview { get; }
}