using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb")]
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