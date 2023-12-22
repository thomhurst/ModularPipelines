using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql")]
public class AzCosmosdbSqlRole
{
    public AzCosmosdbSqlRole(
        AzCosmosdbSqlRoleAssignment assignment,
        AzCosmosdbSqlRoleDefinition definition
    )
    {
        Assignment = assignment;
        Definition = definition;
    }

    public AzCosmosdbSqlRoleAssignment Assignment { get; }

    public AzCosmosdbSqlRoleDefinition Definition { get; }
}