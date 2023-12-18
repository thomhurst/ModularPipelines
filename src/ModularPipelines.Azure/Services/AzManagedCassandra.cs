using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzManagedCassandra
{
    public AzManagedCassandra(
        AzManagedCassandraCluster cluster,
        AzManagedCassandraDatacenter datacenter
    )
    {
        Cluster = cluster;
        Datacenter = datacenter;
    }

    public AzManagedCassandraCluster Cluster { get; }

    public AzManagedCassandraDatacenter Datacenter { get; }
}