using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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