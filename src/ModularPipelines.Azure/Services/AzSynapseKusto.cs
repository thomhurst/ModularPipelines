using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse")]
public class AzSynapseKusto
{
    public AzSynapseKusto(
        AzSynapseKustoAttachedDatabaseConfiguration attachedDatabaseConfiguration,
        AzSynapseKustoDataConnection dataConnection,
        AzSynapseKustoDatabase database,
        AzSynapseKustoDatabasePrincipalAssignment databasePrincipalAssignment,
        AzSynapseKustoPool pool,
        AzSynapseKustoPoolPrincipalAssignment poolPrincipalAssignment
    )
    {
        AttachedDatabaseConfiguration = attachedDatabaseConfiguration;
        DataConnection = dataConnection;
        Database = database;
        DatabasePrincipalAssignment = databasePrincipalAssignment;
        Pool = pool;
        PoolPrincipalAssignment = poolPrincipalAssignment;
    }

    public AzSynapseKustoAttachedDatabaseConfiguration AttachedDatabaseConfiguration { get; }

    public AzSynapseKustoDataConnection DataConnection { get; }

    public AzSynapseKustoDatabase Database { get; }

    public AzSynapseKustoDatabasePrincipalAssignment DatabasePrincipalAssignment { get; }

    public AzSynapseKustoPool Pool { get; }

    public AzSynapseKustoPoolPrincipalAssignment PoolPrincipalAssignment { get; }
}