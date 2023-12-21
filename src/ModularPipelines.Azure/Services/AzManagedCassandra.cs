using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

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