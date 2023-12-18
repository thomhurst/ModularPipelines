using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzKusto
{
    public AzKusto(
        AzKustoAttachedDatabaseConfiguration attachedDatabaseConfiguration,
        AzKustoCluster cluster,
        AzKustoClusterPrincipalAssignment clusterPrincipalAssignment,
        AzKustoDataConnection dataConnection,
        AzKustoDatabase database,
        AzKustoDatabasePrincipalAssignment databasePrincipalAssignment,
        AzKustoManagedPrivateEndpoint managedPrivateEndpoint,
        AzKustoOperationResult operationResult,
        AzKustoOperationResultLocation operationResultLocation,
        AzKustoPrivateEndpointConnection privateEndpointConnection,
        AzKustoPrivateLinkResource privateLinkResource,
        AzKustoScript script
    )
    {
        AttachedDatabaseConfiguration = attachedDatabaseConfiguration;
        Cluster = cluster;
        ClusterPrincipalAssignment = clusterPrincipalAssignment;
        DataConnection = dataConnection;
        Database = database;
        DatabasePrincipalAssignment = databasePrincipalAssignment;
        ManagedPrivateEndpoint = managedPrivateEndpoint;
        OperationResult = operationResult;
        OperationResultLocation = operationResultLocation;
        PrivateEndpointConnection = privateEndpointConnection;
        PrivateLinkResource = privateLinkResource;
        Script = script;
    }

    public AzKustoAttachedDatabaseConfiguration AttachedDatabaseConfiguration { get; }

    public AzKustoCluster Cluster { get; }

    public AzKustoClusterPrincipalAssignment ClusterPrincipalAssignment { get; }

    public AzKustoDataConnection DataConnection { get; }

    public AzKustoDatabase Database { get; }

    public AzKustoDatabasePrincipalAssignment DatabasePrincipalAssignment { get; }

    public AzKustoManagedPrivateEndpoint ManagedPrivateEndpoint { get; }

    public AzKustoOperationResult OperationResult { get; }

    public AzKustoOperationResultLocation OperationResultLocation { get; }

    public AzKustoPrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzKustoPrivateLinkResource PrivateLinkResource { get; }

    public AzKustoScript Script { get; }
}

