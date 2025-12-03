using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "mongodb")]
public class AzCosmosdbMongodbRole
{
    public AzCosmosdbMongodbRole(
        AzCosmosdbMongodbRoleDefinition definition
    )
    {
        Definition = definition;
    }

    public AzCosmosdbMongodbRoleDefinition Definition { get; }
}